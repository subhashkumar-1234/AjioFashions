using System.Collections.Generic;

namespace Ecom.Application.DTOs
{
    public class DashboardStatsDTO
    {
        public decimal TotalRevenue { get; set; }
        public int TotalOrders { get; set; }
        public int LowStockCount { get; set; }
        public int ActiveCustomersCount { get; set; }
        public List<CategoryShareDTO> CategoryShares { get; set; } = new List<CategoryShareDTO>();
        public List<LowStockProductDTO> TopLowStockProducts { get; set; } = new List<LowStockProductDTO>();
    }

    public class CategoryShareDTO
    {
        public string CategoryName { get; set; } = string.Empty;
        public decimal Revenue { get; set; }
        public decimal SharePercentage { get; set; }
    }

    public class LowStockProductDTO
    {
        public int Id { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public string CategoryName { get; set; } = string.Empty;
        public int Stock { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
    }
}
