using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Application.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<CartDTO> GetCartByUserIdAsync(int userId)
        {
            var cart = await _cartRepository.GetOrCreateCartByUserIdAsync(userId);
            return MapToDTO(cart);
        }

        public async Task<CartDTO> AddOrUpdateCartItemAsync(int userId, CartItemUpdateDTO itemUpdate)
        {
            var cartItem = new CartItem
            {
                ProductId = itemUpdate.ProductId,
                Quantity = itemUpdate.Quantity,
                Size = itemUpdate.Size
            };
            var cart = await _cartRepository.AddOrUpdateCartItemAsync(userId, cartItem);
            return MapToDTO(cart);
        }

        public async Task<CartDTO> RemoveCartItemAsync(int userId, int productId)
        {
            var cart = await _cartRepository.RemoveCartItemAsync(userId, productId);
            return MapToDTO(cart);
        }

        public async Task<CartDTO> ClearCartAsync(int userId)
        {
            var cart = await _cartRepository.ClearCartAsync(userId);
            return MapToDTO(cart);
        }

        public async Task<CartDTO> SyncCartAsync(int userId, CartSyncDTO syncDto)
        {
            // First clear current db cart
            await _cartRepository.ClearCartAsync(userId);

            // Add all items from local storage
            if (syncDto.Items != null && syncDto.Items.Any())
            {
                foreach (var item in syncDto.Items)
                {
                    var cartItem = new CartItem
                    {
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        Size = item.Size
                    };
                    await _cartRepository.AddOrUpdateCartItemAsync(userId, cartItem);
                }
            }

            return await GetCartByUserIdAsync(userId);
        }

        private CartDTO MapToDTO(Cart cart)
        {
            return new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                Items = cart.CartItems.Select(ci => new CartItemDTO
                {
                    Id = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.ProductName ?? "Unknown Product",
                    Price = ci.Product?.ItemPrice ?? 0,
                    ImageUrl = ci.Product?.ImageUrl ?? string.Empty,
                    Quantity = ci.Quantity,
                    Size = ci.Size,
                    SellerId = ci.Product != null ? ci.Product.SellerId : null
                }).ToList()
            };
        }
    }
}
