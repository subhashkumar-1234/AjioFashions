namespace Ecom.Application.DTOs
{
    public class ReviewCreateDTO
    {
        public int ProductId { get; set; }
        public int Rating { get; set; } // 1-5
        public string Comment { get; set; } = string.Empty;
    }

    public class ReviewResponseDTO
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string Comment { get; set; } = string.Empty;
        public System.DateTime CreatedAt { get; set; }
    }
}
