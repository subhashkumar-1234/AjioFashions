using Ecom.Application.DTOs;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IWishlistService
    {
        Task<WishlistDto> GetWishlistByUserIdAsync(int userId);
        Task<WishlistDto> AddToWishlistAsync(int userId, int productId);
        Task<WishlistDto> RemoveFromWishlistAsync(int userId, int productId);
        Task<WishlistDto> SyncWishlistAsync(int userId, WishlistSyncDto syncDto);
        Task<WishlistDto> ClearWishlistAsync(int userId);
    }
}
