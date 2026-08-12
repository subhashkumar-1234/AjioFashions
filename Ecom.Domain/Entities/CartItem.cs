using System;

namespace Ecom.Domain.Entities
{
    public class CartItem
    {
        public int Id { get; set; }
        
        public int CartId { get; set; }
        public Cart Cart { get; set; } = null!;
        
        public int ProductId { get; set; } // Foreign key to AddItem
        public AddItem Product { get; set; } = null!;
        
        public int Quantity { get; set; }
        public string Size { get; set; } = "M";
    }
}
