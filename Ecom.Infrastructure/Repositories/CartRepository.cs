using Microsoft.EntityFrameworkCore;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;

        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetOrCreateCartByUserIdAsync(int userId)
        {
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.Carts.Add(cart);
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> AddOrUpdateCartItemAsync(int userId, CartItem item)
        {
            var cart = await GetOrCreateCartByUserIdAsync(userId);
            var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == item.ProductId && ci.Size == item.Size);

            if (existingItem != null)
            {
                existingItem.Quantity = item.Quantity;
            }
            else
            {
                item.CartId = cart.Id;
                _context.CartItems.Add(item);
                cart.CartItems.Add(item);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> RemoveCartItemAsync(int userId, int productId)
        {
            var cart = await GetOrCreateCartByUserIdAsync(userId);
            var itemsToRemove = cart.CartItems.Where(ci => ci.ProductId == productId).ToList();

            if (itemsToRemove.Any())
            {
                _context.CartItems.RemoveRange(itemsToRemove);
                foreach (var rem in itemsToRemove)
                {
                    cart.CartItems.Remove(rem);
                }
                cart.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return cart;
        }

        public async Task<Cart> ClearCartAsync(int userId)
        {
            var cart = await GetOrCreateCartByUserIdAsync(userId);
            if (cart.CartItems.Any())
            {
                _context.CartItems.RemoveRange(cart.CartItems);
                cart.CartItems.Clear();
                cart.UpdatedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }

            return cart;
        }
    }
}
