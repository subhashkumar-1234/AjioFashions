using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Interfaces.AllIteam
{
    public interface IAddItemRepository
    {
        public Task<IEnumerable<AddItem>> GetAllAddItemsAsync();
        public Task<AddItem?> GetAddItemByIdAsync(int id);
        public Task<AddItem> CreateAddItemAsync(AddItem addItem);
        public Task<AddItem> UpdateAddItemAsync( AddItem addItem);
        public Task<bool> DeleteAddItemAsync(int id);
        public Task<(IEnumerable<AddItem> Items, int TotalCount)> GetPagedItemsAsync(string? search, int? categoryId, string? size, string? sortBy, int page, int pageSize);
    }
}
