using System.Collections.Generic;

namespace Ecom.Application.DTOs
{
    public class OrderCreateDTO
    {
        public required string ShippingAddress { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PostalCode { get; set; }
        public List<OrderItemCreateDTO> Items { get; set; } = new List<OrderItemCreateDTO>();
        public string? PaymentId { get; set; }
        public string? CouponCode { get; set; }
    }

    public class OrderItemCreateDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string Size { get; set; } = "M";
    }
}
