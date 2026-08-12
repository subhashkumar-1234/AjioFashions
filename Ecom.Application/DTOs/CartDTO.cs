using System.Collections.Generic;

namespace Ecom.Application.DTOs
{
    public class CartDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public List<CartItemDTO> Items { get; set; } = new List<CartItemDTO>();
    }

    public class CartItemDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public string Size { get; set; } = "M";
        public int? SellerId { get; set; }
    }

    public class CartItemUpdateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; } = "M";
    }

    public class CartSyncDTO
    {
        public List<CartItemUpdateDTO> Items { get; set; } = new List<CartItemUpdateDTO>();
    }
}
