using System;
using System.Collections.Generic;

namespace Ecom.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        
        public int UserId { get; set; }
        public User User { get; set; } = null!;
        
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        
        public required string ShippingAddress { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PostalCode { get; set; }
        
        public decimal TotalAmount { get; set; }
        
        public string Status { get; set; } = "PENDING"; // PENDING, PAID, CANCELLED, DELIVERED
        
        public string? PaymentId { get; set; } // For tracking Stripe/Razorpay payment
        
        // Delivery / Logistics Agent Mapping
        public int? DeliveryAgentId { get; set; }
        public User? DeliveryAgent { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
