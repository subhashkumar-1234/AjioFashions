using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IAddItemRepository _addItemRepository;
        private readonly IUserRepository _userRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IEmailService _emailService;
        private readonly ICouponRepository _couponRepository;
        private readonly IProductSizeStockRepository _productSizeStockRepository;

        public OrderService(
            IOrderRepository orderRepository,
            IAddItemRepository addItemRepository,
            IUserRepository userRepository,
            ICartRepository cartRepository,
            IEmailService emailService,
            ICouponRepository couponRepository,
            IProductSizeStockRepository productSizeStockRepository)
        {
            _orderRepository = orderRepository;
            _addItemRepository = addItemRepository;
            _userRepository = userRepository;
            _cartRepository = cartRepository;
            _emailService = emailService;
            _couponRepository = couponRepository;
            _productSizeStockRepository = productSizeStockRepository;
        }

        public async Task<OrderResponseDTO> CreateOrderAsync(int userId, OrderCreateDTO orderDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.UtcNow,
                ShippingAddress = orderDto.ShippingAddress,
                PhoneNumber = orderDto.PhoneNumber,
                PostalCode = orderDto.PostalCode,
                Status = "PENDING",
                PaymentId = orderDto.PaymentId,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var itemDto in orderDto.Items)
            {
                var product = await _addItemRepository.GetAddItemByIdAsync(itemDto.ProductId);
                if (product == null)
                {
                    throw new Exception($"Product with ID {itemDto.ProductId} not found");
                }

                // Check size-specific stock availability
                var sizeStockAvailable = await _productSizeStockRepository.CheckStockAsync(itemDto.ProductId, itemDto.Size, itemDto.Quantity);
                if (!sizeStockAvailable)
                {
                    var sizeStock = await _productSizeStockRepository.GetSizeStockAsync(itemDto.ProductId, itemDto.Size);
                    int currentStock = sizeStock?.Stock ?? 0;
                    throw new Exception($"Insufficient stock for product '{product.ProductName}' size '{itemDto.Size}'. Available: {currentStock}, Requested: {itemDto.Quantity}");
                }

                // Deduct size-specific stock (this repository method also syncs aggregate product stock)
                await _productSizeStockRepository.DecrementStockAsync(itemDto.ProductId, itemDto.Size, itemDto.Quantity);

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Product = product, // Assign for HTML mapping and email template
                    Quantity = itemDto.Quantity,
                    Price = product.ItemPrice,
                    Size = itemDto.Size
                };

                order.OrderItems.Add(orderItem);
                totalAmount += product.ItemPrice * itemDto.Quantity;
            }

            if (!string.IsNullOrWhiteSpace(orderDto.CouponCode))
            {
                var coupon = await _couponRepository.GetCouponByCodeAsync(orderDto.CouponCode);
                if (coupon != null && coupon.IsActive && coupon.ExpiryDate > DateTime.UtcNow)
                {
                    if (coupon.SellerId.HasValue)
                    {
                        decimal sellerSubtotal = 0;
                        foreach (var orderItem in order.OrderItems)
                        {
                            var product = await _addItemRepository.GetAddItemByIdAsync(orderItem.ProductId);
                            if (product != null && product.SellerId == coupon.SellerId.Value)
                            {
                                sellerSubtotal += orderItem.Price * orderItem.Quantity;
                            }
                        }
                        totalAmount -= sellerSubtotal * (coupon.DiscountPercentage / 100);
                    }
                    else
                    {
                        totalAmount -= totalAmount * (coupon.DiscountPercentage / 100);
                    }
                }
            }

            order.TotalAmount = totalAmount;

            var createdOrder = await _orderRepository.AddOrderAsync(order);

            // Clear database cart
            await _cartRepository.ClearCartAsync(userId);

            // Send HTML Invoice Email
            try
            {
                var itemsRows = string.Join("", order.OrderItems.Select(oi => $@"
                    <tr style='border-bottom: 1px solid #f1f5f9;'>
                        <td style='padding: 12px 0; color: #374151;'>{(oi.Product?.ProductName ?? $"Product #{oi.ProductId}")} (Size: {oi.Size})</td>
                        <td style='padding: 12px 0; text-align: center; color: #374151;'>{oi.Quantity}</td>
                        <td style='padding: 12px 0; text-align: right; color: #374151;'>${(oi.Price * oi.Quantity):F2}</td>
                    </tr>"));

                var htmlContent = $@"
                    <div style='font-family: Arial, sans-serif; max-width: 600px; margin: 0 auto; border: 1px solid #e2e8f0; border-radius: 12px; overflow: hidden; box-shadow: 0 4px 12px rgba(0,0,0,0.05);'>
                        <div style='background: linear-gradient(135deg, #4f46e5, #3b82f6); padding: 32px; text-align: center; color: white;'>
                            <h1 style='margin: 0; font-size: 28px; font-weight: 700;'>Order Confirmed!</h1>
                            <p style='margin: 8px 0 0 0; opacity: 0.9;'>Thank you for your purchase.</p>
                        </div>
                        <div style='padding: 32px;'>
                            <h3 style='margin-top: 0; color: #1f2937;'>Order #{createdOrder.Id}</h3>
                            <p style='color: #4b5563; font-size: 14px;'>Placed on {createdOrder.OrderDate.ToString("f")}</p>
                            <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 24px 0;' />
                            <h4 style='margin: 0 0 12px 0; color: #374151;'>Shipping Details</h4>
                            <p style='margin: 4px 0; color: #4b5563; font-size: 14px;'><strong>Address:</strong> {createdOrder.ShippingAddress}</p>
                            <p style='margin: 4px 0; color: #4b5563; font-size: 14px;'><strong>Phone:</strong> {createdOrder.PhoneNumber}</p>
                            <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 24px 0;' />
                            <h4 style='margin: 0 0 12px 0; color: #374151;'>Items Ordered</h4>
                            <table style='width: 100%; border-collapse: collapse;'>
                                <thead>
                                    <tr style='border-bottom: 2px solid #e2e8f0;'>
                                        <th style='text-align: left; padding: 8px 0; color: #4b5563;'>Item</th>
                                        <th style='text-align: center; padding: 8px 0; color: #4b5563;'>Qty</th>
                                        <th style='text-align: right; padding: 8px 0; color: #4b5563;'>Price</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {itemsRows}
                                </tbody>
                            </table>
                            <hr style='border: none; border-top: 1px solid #e2e8f0; margin: 24px 0;' />
                            <div style='text-align: right;'>
                                <p style='font-size: 18px; font-weight: 700; color: #1f2937; margin: 0;'>Total: ${createdOrder.TotalAmount:F2}</p>
                            </div>
                        </div>
                    </div>";

                await _emailService.SendEmailAsync(user.Email, $"Order Receipt - #{createdOrder.Id}", htmlContent);
            }
            catch (Exception emailEx)
            {
                Console.WriteLine($"[ORDER EMAIL INVOICE ERROR] Failed to send receipt email: {emailEx.Message}");
            }

            return MapToResponseDTO(createdOrder, user);
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByUserIdAsync(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) return Enumerable.Empty<OrderResponseDTO>();

            var orders = await _orderRepository.GetOrdersByUserIdAsync(userId);
            return orders.Select(o => MapToResponseDTO(o, user));
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetAllOrdersAsync()
        {
            var orders = await _orderRepository.GetAllOrdersAsync();
            var responseList = new List<OrderResponseDTO>();

            foreach (var o in orders)
            {
                var user = await _userRepository.GetUserByIdAsync(o.UserId);
                responseList.Add(MapToResponseDTO(o, user));
            }

            return responseList;
        }

        public async Task<OrderResponseDTO?> GetOrderByIdAsync(int id)
        {
            var order = await _orderRepository.GetOrderByIdAsync(id);
            if (order == null) return null;

            var user = await _userRepository.GetUserByIdAsync(order.UserId);
            return MapToResponseDTO(order, user);
        }

        public async Task<bool> CancelOrderAsync(int orderId, int userId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(orderId);
            if (order == null) return false;

            if (order.UserId != userId)
            {
                throw new UnauthorizedAccessException("Not authorized to cancel this order.");
            }

            if (order.Status.ToUpper() == "CANCELLED") return false;

            order.Status = "CANCELLED";

            // Restore Stock
            foreach (var item in order.OrderItems)
            {
                var sizeStock = await _productSizeStockRepository.GetSizeStockAsync(item.ProductId, item.Size);
                int newStock = (sizeStock?.Stock ?? 0) + item.Quantity;
                await _productSizeStockRepository.UpdateSizeStockAsync(item.ProductId, item.Size, newStock);

                var product = await _addItemRepository.GetAddItemByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Stock += item.Quantity;
                    await _addItemRepository.UpdateAddItemAsync(product);
                }
            }

            await _orderRepository.UpdateOrderAsync(order);
            return true;
        }

        private OrderResponseDTO MapToResponseDTO(Order o, User? user)
        {
            return new OrderResponseDTO
            {
                Id = o.Id,
                UserId = o.UserId,
                CustomerEmail = user?.Email ?? "unknown@domain.com",
                CustomerName = user?.Name ?? "Unknown User",
                OrderDate = o.OrderDate,
                ShippingAddress = o.ShippingAddress,
                PhoneNumber = o.PhoneNumber,
                PostalCode = o.PostalCode,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                PaymentId = o.PaymentId,
                DeliveryAgentId = o.DeliveryAgentId,
                OrderItems = o.OrderItems.Select(oi => new OrderItemResponseDTO
                {
                    Id = oi.Id,
                    ProductId = oi.ProductId,
                    ProductName = oi.Product?.ProductName ?? "Unknown Product",
                    ImageUrl = oi.Product?.ImageUrl ?? "",
                    Quantity = oi.Quantity,
                    Price = oi.Price,
                    Size = oi.Size
                }).ToList()
            };
        }
    }
}
