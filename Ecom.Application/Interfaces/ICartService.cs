using Ecom.Application.DTOs;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface ICartService
    {
        Task<CartDTO> GetCartByUserIdAsync(int userId);
        Task<CartDTO> AddOrUpdateCartItemAsync(int userId, CartItemUpdateDTO itemUpdate);
        Task<CartDTO> RemoveCartItemAsync(int userId, int productId);
        Task<CartDTO> ClearCartAsync(int userId);
        Task<CartDTO> SyncCartAsync(int userId, CartSyncDTO syncDto);
    }
}
