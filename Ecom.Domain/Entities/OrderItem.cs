using System;

namespace Ecom.Domain.Entities
{
    public class OrderItem
    {
        public int Id { get; set; }
        
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        
        public int ProductId { get; set; } // Foreign key to AddItem
        public AddItem Product { get; set; } = null!;
        
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Size { get; set; } = string.Empty;
    }
}
