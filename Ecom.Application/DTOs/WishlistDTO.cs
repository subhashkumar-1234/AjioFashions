using System.Collections.Generic;

namespace Ecom.Application.DTOs
{
    public class WishlistDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<WishlistItemDto> Items { get; set; } = new List<WishlistItemDto>();
    }

    public class WishlistItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class WishlistSyncDto
    {
        public List<int> ProductIds { get; set; } = new List<int>();
    }
}
