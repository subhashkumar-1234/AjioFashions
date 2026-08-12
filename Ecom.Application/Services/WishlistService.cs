using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Application.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public async Task<WishlistDto> GetWishlistByUserIdAsync(int userId)
        {
            var wishlist = await _wishlistRepository.GetOrCreateWishlistByUserIdAsync(userId);
            return MapToDTO(wishlist);
        }

        public async Task<WishlistDto> AddToWishlistAsync(int userId, int productId)
        {
            var wishlist = await _wishlistRepository.AddWishlistItemAsync(userId, productId);
            return MapToDTO(wishlist);
        }

        public async Task<WishlistDto> RemoveFromWishlistAsync(int userId, int productId)
        {
            var wishlist = await _wishlistRepository.RemoveWishlistItemAsync(userId, productId);
            return MapToDTO(wishlist);
        }

        public async Task<WishlistDto> ClearWishlistAsync(int userId)
        {
            var wishlist = await _wishlistRepository.ClearWishlistAsync(userId);
            return MapToDTO(wishlist);
        }

        public async Task<WishlistDto> SyncWishlistAsync(int userId, WishlistSyncDto syncDto)
        {
            if (syncDto.ProductIds != null && syncDto.ProductIds.Any())
            {
                foreach (var productId in syncDto.ProductIds)
                {
                    await _wishlistRepository.AddWishlistItemAsync(userId, productId);
                }
            }

            return await GetWishlistByUserIdAsync(userId);
        }

        private WishlistDto MapToDTO(Wishlist wishlist)
        {
            return new WishlistDto
            {
                Id = wishlist.Id,
                UserId = wishlist.UserId,
                Items = wishlist.WishlistItems.Select(wi => new WishlistItemDto
                {
                    Id = wi.Id,
                    ProductId = wi.ProductId,
                    ProductName = wi.Product?.ProductName ?? "Unknown Product",
                    Price = wi.Product?.ItemPrice ?? 0,
                    ImageUrl = wi.Product?.ImageUrl ?? string.Empty,
                    Description = wi.Product?.ItemDescription ?? string.Empty
                }).ToList()
            };
        }
    }
}
