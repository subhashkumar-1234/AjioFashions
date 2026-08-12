namespace Ecom.Application.DTOs
{
    public class AddressDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AddressLine { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PostalCode { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
    }

    public class AddressCreateDTO
    {
        public required string AddressLine { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PostalCode { get; set; }
        public bool IsDefault { get; set; }
    }
}
