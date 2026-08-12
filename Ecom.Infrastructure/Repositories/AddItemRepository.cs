using System;
using System.Collections.Generic;
using System.Text;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Ecom.Infrastructure.Repositories
{
    public class AddItemRepository : IAddItemRepository
    {
        private readonly AppDbContext _context;
        public AddItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AddItem>> GetAllAddItemsAsync()
        {
            return await _context.AddItems.AsNoTracking().ToListAsync();
        }
        public async Task<AddItem?> GetAddItemByIdAsync(int id)
        {
            return await _context.AddItems.FindAsync(id);
        }
        public async Task<AddItem> CreateAddItemAsync(AddItem addItem)
        {
            await _context.AddItems.AddAsync(addItem);
            await _context.SaveChangesAsync();

            return addItem;
        }
        public async Task<AddItem> UpdateAddItemAsync(AddItem addItem)
        {
            _context.AddItems.Update(addItem);
            await _context.SaveChangesAsync();
            return addItem;
        }
        public async Task<bool> DeleteAddItemAsync(int id)
        {
            var addItem = await _context.AddItems.FindAsync(id);
            if (addItem == null) return false;
            _context.AddItems.Remove(addItem);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<(IEnumerable<AddItem> Items, int TotalCount)> GetPagedItemsAsync(string? search, int? categoryId, string? size, string? sortBy, int page, int pageSize)
        {
            var query = _context.AddItems.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(ai => ai.ProductName.ToLower().Contains(s) || ai.ItemDescription.ToLower().Contains(s));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(ai => ai.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(size))
            {
                var sz = size.Trim().ToLower();
                query = query.Where(ai => ai.Size.ToLower().Contains(sz));
            }

            switch (sortBy?.ToLower())
            {
                case "price_asc":
                    query = query.OrderBy(ai => ai.ItemPrice);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(ai => ai.ItemPrice);
                    break;
                case "name_asc":
                    query = query.OrderBy(ai => ai.ProductName);
                    break;
                case "name_desc":
                    query = query.OrderByDescending(ai => ai.ProductName);
                    break;
                default:
                    query = query.OrderBy(ai => ai.Id);
                    break;
            }

            int totalCount = await query.CountAsync();
            
            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
