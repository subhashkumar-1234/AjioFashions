namespace Ecom.Application.DTOs
{
    public class ProductSizeStockDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Size { get; set; } = string.Empty;
        public int Stock { get; set; }
    }
}
