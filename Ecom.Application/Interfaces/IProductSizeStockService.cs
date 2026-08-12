using Ecom.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IProductSizeStockService
    {
        Task<IEnumerable<ProductSizeStockDto>> GetSizeStocksByProductIdAsync(int productId);
        Task<ProductSizeStockDto> UpdateSizeStockAsync(ProductSizeStockDto dto);
        Task<bool> CheckStockAsync(int productId, string size, int quantity);
    }
}
