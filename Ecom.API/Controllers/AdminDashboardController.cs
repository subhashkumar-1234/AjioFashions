using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Ecom.Infrastructure.Data;
using Ecom.Application.DTOs;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using System;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class AdminDashboardController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AdminDashboardController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetDashboardReports()
        {
            try
            {
                var orders = await _context.Orders
                    .Where(o => o.Status != "CANCELLED")
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .ToListAsync();

                var totalRevenue = orders.Sum(o => o.TotalAmount);
                var totalOrders = orders.Count;

                var lowStockThreshold = 10;
                var lowStockProducts = await _context.AddItems
                    .Where(ai => ai.Stock < lowStockThreshold)
                    .Select(ai => new LowStockProductDTO
                    {
                        Id = ai.Id,
                        ProductName = ai.ProductName,
                        CategoryName = _context.Categories.Where(c => c.Id == ai.CategoryId).Select(c => c.CategoryName).FirstOrDefault() ?? "Uncategorized",
                        Stock = ai.Stock,
                        ImageUrl = ai.ImageUrl ?? string.Empty
                    })
                    .ToListAsync();

                var lowStockCount = await _context.AddItems.CountAsync(ai => ai.Stock < lowStockThreshold);
                var activeCustomersCount = orders.Select(o => o.UserId).Distinct().Count();

                var itemRevenueList = orders
                    .SelectMany(o => o.OrderItems)
                    .Where(oi => oi.Product != null)
                    .ToList();

                var categoryRevenue = itemRevenueList
                    .GroupBy(oi => oi.Product.CategoryId)
                    .Select(g => new
                    {
                        CategoryId = g.Key,
                        Revenue = g.Sum(oi => oi.Price * oi.Quantity)
                    })
                    .ToList();

                var categoryShares = new List<CategoryShareDTO>();
                var sumRevenue = categoryRevenue.Sum(cr => cr.Revenue);
                foreach (var cr in categoryRevenue)
                {
                    var catName = _context.Categories.Where(c => c.Id == cr.CategoryId).Select(c => c.CategoryName).FirstOrDefault() ?? "Uncategorized";
                    categoryShares.Add(new CategoryShareDTO
                    {
                        CategoryName = catName,
                        Revenue = cr.Revenue,
                        SharePercentage = sumRevenue > 0 ? Math.Round((cr.Revenue / sumRevenue) * 100, 2) : 0
                    });
                }

                var stats = new DashboardStatsDTO
                {
                    TotalRevenue = totalRevenue,
                    TotalOrders = totalOrders,
                    LowStockCount = lowStockCount,
                    ActiveCustomersCount = activeCustomersCount,
                    CategoryShares = categoryShares,
                    TopLowStockProducts = lowStockProducts.Take(5).ToList()
                };

                return Ok(stats);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
