using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Domain.Entities
{
    public class AddItem
    {
        public int Id { get; set; }

        public string ProductName { get; set; }
        public string ItemDescription { get; set; }

        public decimal ItemPrice { get; set; } = 0;

        public int Stock { get; set; }

        public string Size { get; set; }

        public string ImageUrl { get; set; }

        // Foreign Key
        public int CategoryId { get; set; }

        // Navigation Property
        public Category Category { get; set; }

        // Seller/Merchant Association
        public int? SellerId { get; set; }
        public User? Seller { get; set; }

        public ICollection<AddQuantity> AddQuantities { get; set; } = new List<AddQuantity>();
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    }
}
