using System;

namespace Ecom.Domain.Entities
{
    public class ProductSizeStock
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; }
        public AddItem Product { get; set; } = null!;
        
        public string Size { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
