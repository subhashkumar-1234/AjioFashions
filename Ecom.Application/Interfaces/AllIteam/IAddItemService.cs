using Ecom.Application.DTOs;
using Ecom.Application.DTOs.AllItemDtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces.AllIteam
{
    public interface IAddItemService
    {
        public Task<IEnumerable<AddItemDto>> GetAllAddItemsAsync();
        public Task<AddItemDto?> GetAddItemByIdAsync(int id);
        public Task<AddItemDto> CreateAddItemAsync(AddItemDto addItemDto);
        public Task<AddItemDto> UpdateAddItemAsync(int id, UpdateItemDto updateItemDto);
        public Task<bool> DeleteAddItemAsync(int id);
        public Task<PagedResponseDTO<AddItemDto>> GetPagedItemsAsync(string? search, int? categoryId, string? size, string? sortBy, int page, int pageSize);
    }
}
