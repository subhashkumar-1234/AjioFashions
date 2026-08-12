using Ecom.Domain.Entities;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IWishlistRepository
    {
        Task<Wishlist> GetOrCreateWishlistByUserIdAsync(int userId);
        Task<Wishlist> AddWishlistItemAsync(int userId, int productId);
        Task<Wishlist> RemoveWishlistItemAsync(int userId, int productId);
        Task<Wishlist> ClearWishlistAsync(int userId);
    }
}
