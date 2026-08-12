using Ecom.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IProductSizeStockRepository
    {
        Task<IEnumerable<ProductSizeStock>> GetSizeStocksByProductIdAsync(int productId);
        Task<ProductSizeStock?> GetSizeStockAsync(int productId, string size);
        Task<ProductSizeStock> UpdateSizeStockAsync(int productId, string size, int stock);
        Task<bool> CheckStockAsync(int productId, string size, int quantity);
        Task<bool> DecrementStockAsync(int productId, string size, int quantity);
    }
}
