using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Ecom.Infrastructure.Data;
using Ecom.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Seller")]
    public class SellerController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SellerController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetSellerReports()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid token credentials");

                // Fetch active order items belonging to this seller
                var sellerOrderItems = await _context.OrderItems
                    .Include(oi => oi.Order)
                    .Include(oi => oi.Product)
                    .Where(oi => oi.Product != null && oi.Product.SellerId == userId && oi.Order.Status != "CANCELLED")
                    .ToListAsync();

                var totalRevenue = sellerOrderItems.Sum(oi => oi.Price * oi.Quantity);
                var totalOrders = sellerOrderItems.Select(oi => oi.OrderId).Distinct().Count();
                var activeCustomersCount = sellerOrderItems.Select(oi => oi.Order.UserId).Distinct().Count();

                // Low stock count for this seller
                var lowStockThreshold = 10;
                var sellerLowStockProducts = await _context.AddItems
                    .Where(p => p.SellerId == userId && p.Stock < lowStockThreshold)
                    .Select(p => new
                    {
                        p.Id,
                        ProductName = p.ProductName,
                        CategoryName = _context.Categories.Where(c => c.Id == p.CategoryId).Select(c => c.CategoryName).FirstOrDefault() ?? "Uncategorized",
                        p.Stock,
                        ImageUrl = p.ImageUrl ?? string.Empty
                    })
                    .ToListAsync();

                var lowStockCount = sellerLowStockProducts.Count;

                // Category share calculation
                var categoryRevenue = sellerOrderItems
                    .GroupBy(oi => oi.Product.CategoryId)
                    .Select(g => new
                    {
                        CategoryId = g.Key,
                        Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                    })
                    .ToList();

                var categoryShares = new List<object>();
                foreach (var cr in categoryRevenue)
                {
                    var catName = _context.Categories.Where(c => c.Id == cr.CategoryId).Select(c => c.CategoryName).FirstOrDefault() ?? "Uncategorized";
                    categoryShares.Add(new
                    {
                        CategoryName = catName,
                        Revenue = cr.Revenue,
                        SharePercentage = totalRevenue > 0 ? Math.Round((double)(cr.Revenue / totalRevenue) * 100, 2) : 0
                    });
                }

                var stats = new
                {
                    TotalRevenue = totalRevenue,
                    TotalOrders = totalOrders,
                    LowStockCount = lowStockCount,
                    ActiveCustomersCount = activeCustomersCount,
                    CategoryShares = categoryShares,
                    TopLowStockProducts = sellerLowStockProducts.Take(5).ToList()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("products")]
        public async Task<IActionResult> GetSellerProducts()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid token credentials");

                var products = await _context.AddItems
                    .Where(p => p.SellerId == userId)
                    .Select(p => new
                    {
                        p.Id,
                        p.ProductName,
                        p.ItemDescription,
                        p.ItemPrice,
                        p.Stock,
                        p.Size,
                        p.ImageUrl,
                        p.CategoryId,
                        CategoryName = _context.Categories.Where(c => c.Id == p.CategoryId).Select(c => c.CategoryName).FirstOrDefault() ?? "Uncategorized"
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("products")]
        public async Task<IActionResult> AddSellerProduct([FromBody] SellerProductCreateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid token credentials");

                var product = new AddItem
                {
                    ProductName = dto.ProductName,
                    ItemDescription = dto.ItemDescription,
                    ItemPrice = dto.ItemPrice,
                    Stock = dto.Stock,
                    Size = dto.Size,
                    ImageUrl = dto.ImageUrl ?? string.Empty,
                    CategoryId = dto.CategoryId,
                    SellerId = userId
                };

                _context.AddItems.Add(product);
                await _context.SaveChangesAsync();

                // Seed default size stocks mapping
                _context.ProductSizeStocks.AddRange(new[]
                {
                    new ProductSizeStock { ProductId = product.Id, Size = "S", Stock = product.Stock / 4 },
                    new ProductSizeStock { ProductId = product.Id, Size = "M", Stock = product.Stock / 4 },
                    new ProductSizeStock { ProductId = product.Id, Size = "L", Stock = product.Stock / 4 },
                    new ProductSizeStock { ProductId = product.Id, Size = "XL", Stock = product.Stock - (product.Stock / 4 * 3) }
                });
                await _context.SaveChangesAsync();

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("products/{id}")]
        public async Task<IActionResult> UpdateSellerProduct(int id, [FromBody] SellerProductUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid token credentials");

                var product = await _context.AddItems.FirstOrDefaultAsync(p => p.Id == id && p.SellerId == userId);
                if (product == null) return NotFound("Product not found or unauthorized");

                product.ProductName = dto.ProductName;
                product.ItemDescription = dto.ItemDescription;
                product.ItemPrice = dto.ItemPrice;
                product.Stock = dto.Stock;
                product.Size = dto.Size;
                if (dto.ImageUrl != null) product.ImageUrl = dto.ImageUrl;

                product.CategoryId = dto.CategoryId;
                product.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetSellerOrders()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
                if (userId == 0) return Unauthorized("Invalid token credentials");

                // Get orders containing this seller's products
                var orders = await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.OrderItems.Any(oi => oi.Product != null && oi.Product.SellerId == userId))
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();

                var result = orders.Select(o => new
                {
                    o.Id,
                    CustomerEmail = o.User != null ? o.User.Email : "customer@ecom.com",
                    OrderDate = o.OrderDate,
                    ShippingAddress = o.ShippingAddress,
                    PhoneNumber = o.PhoneNumber,
                    PostalCode = o.PostalCode,
                    Status = o.Status,
                    TotalAmount = o.OrderItems.Where(oi => oi.Product != null && oi.Product.SellerId == userId).Sum(oi => oi.Price * oi.Quantity),
                    Items = o.OrderItems
                        .Where(oi => oi.Product != null && oi.Product.SellerId == userId)
                        .Select(oi => new
                        {
                            oi.Id,
                            oi.ProductId,
                            ProductName = oi.Product != null ? oi.Product.ProductName : "Unknown Product",
                            oi.Quantity,
                            oi.Size,
                            oi.Price
                        }).ToList()
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }

    public class SellerProductCreateDto
    {
        public required string ProductName { get; set; }
        public required string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public int Stock { get; set; }
        public required string Size { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }

    public class SellerProductUpdateDto
    {
        public required string ProductName { get; set; }
        public required string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public int Stock { get; set; }
        public required string Size { get; set; }
        public string? ImageUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
