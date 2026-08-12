using Ecom.Domain.Entities;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface ICartRepository
    {
        Task<Cart> GetOrCreateCartByUserIdAsync(int userId);
        Task<Cart> AddOrUpdateCartItemAsync(int userId, CartItem item);
        Task<Cart> RemoveCartItemAsync(int userId, int productId);
        Task<Cart> ClearCartAsync(int userId);
    }
}
