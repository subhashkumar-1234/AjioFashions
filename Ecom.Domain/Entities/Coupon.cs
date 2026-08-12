using System;

namespace Ecom.Domain.Entities
{
    public class Coupon
    {
        public int Id { get; set; }
        
        public required string Code { get; set; } // e.g. WELCOME50
        public decimal DiscountPercentage { get; set; } // e.g. 10.00 for 10%
        
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;

        public int? SellerId { get; set; }
        public User? Seller { get; set; }
    }
}
