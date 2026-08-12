using Microsoft.EntityFrameworkCore;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;

        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Wishlist> GetOrCreateWishlistByUserIdAsync(int userId)
        {
            var wishlist = await _context.Wishlists
                .Include(w => w.WishlistItems)
                .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(w => w.UserId == userId);

            if (wishlist == null)
            {
                wishlist = new Wishlist
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Wishlists.Add(wishlist);
                await _context.SaveChangesAsync();
            }

            return wishlist;
        }

        public async Task<Wishlist> AddWishlistItemAsync(int userId, int productId)
        {
            var wishlist = await GetOrCreateWishlistByUserIdAsync(userId);
            var existingItem = wishlist.WishlistItems.FirstOrDefault(wi => wi.ProductId == productId);

            if (existingItem == null)
            {
                var item = new WishlistItem
                {
                    WishlistId = wishlist.Id,
                    ProductId = productId,
                    AddedAt = DateTime.UtcNow
                };
                _context.WishlistItems.Add(item);
                await _context.SaveChangesAsync();
            }

            return await GetOrCreateWishlistByUserIdAsync(userId);
        }

        public async Task<Wishlist> RemoveWishlistItemAsync(int userId, int productId)
        {
            var wishlist = await GetOrCreateWishlistByUserIdAsync(userId);
            var item = wishlist.WishlistItems.FirstOrDefault(wi => wi.ProductId == productId);

            if (item != null)
            {
                _context.WishlistItems.Remove(item);
                await _context.SaveChangesAsync();
            }

            return await GetOrCreateWishlistByUserIdAsync(userId);
        }

        public async Task<Wishlist> ClearWishlistAsync(int userId)
        {
            var wishlist = await GetOrCreateWishlistByUserIdAsync(userId);
            if (wishlist.WishlistItems.Any())
            {
                _context.WishlistItems.RemoveRange(wishlist.WishlistItems);
                await _context.SaveChangesAsync();
            }

            return await GetOrCreateWishlistByUserIdAsync(userId);
        }
    }
}
