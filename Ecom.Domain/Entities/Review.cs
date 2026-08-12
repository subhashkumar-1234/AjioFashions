using System;

namespace Ecom.Domain.Entities
{
    public class Review
    {
        public int Id { get; set; }
        
        public int ProductId { get; set; } // Foreign key to AddItem
        public AddItem Product { get; set; } = null!;
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public int Rating { get; set; } // 1-5 stars
        public string Comment { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
