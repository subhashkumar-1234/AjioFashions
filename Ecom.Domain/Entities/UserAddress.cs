using System;

namespace Ecom.Domain.Entities
{
    public class UserAddress
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public required string AddressLine { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PostalCode { get; set; }
        
        public bool IsDefault { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
