using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecom.Application.DTOs;
using Ecom.Application.DTOs.AllItemDtos;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Domain.Entities;

namespace Ecom.Application.Services
{
    public class AddItemService : IAddItemService
    {
        private readonly IAddItemRepository _addItemRepository;
        public AddItemService(IAddItemRepository addItemRepository)
        {
            _addItemRepository = addItemRepository;
        }
        public async Task<IEnumerable<AddItemDto>> GetAllAddItemsAsync()
        {
            var addItems = await _addItemRepository.GetAllAddItemsAsync();
            return addItems.Select(ai => new AddItemDto
            {
                Id = ai.Id,
                ProductName = ai.ProductName,
                ItemDescription = ai.ItemDescription,
                ItemPrice = ai.ItemPrice,
                Stock = ai.Stock,
                Size = ai.Size,
                ImageUrl = ai.ImageUrl,
                CategoryId = ai.CategoryId
            });
        }
        public async Task<AddItemDto?> GetAddItemByIdAsync(int id)
        {
            var addItem = await _addItemRepository.GetAddItemByIdAsync(id);
            if (addItem == null) return null;
            return new AddItemDto
            {
                Id = addItem.Id,
                ProductName = addItem.ProductName,
                ItemDescription = addItem.ItemDescription,
                ItemPrice = addItem.ItemPrice,
                Stock = addItem.Stock,
                Size = addItem.Size,
                ImageUrl = addItem.ImageUrl,
                CategoryId = addItem.CategoryId
            };
        }
        public async Task<AddItemDto> CreateAddItemAsync(AddItemDto addItemDto)
        {
            var addItem = new AddItem
            {
                ProductName = addItemDto.ProductName,
                ItemDescription = addItemDto.ItemDescription,
                ItemPrice = addItemDto.ItemPrice,
                Stock = addItemDto.Stock,
                Size = addItemDto.Size,
                ImageUrl = addItemDto.ImageUrl,
                CategoryId = addItemDto.CategoryId

            };
            var createdAddItem = await _addItemRepository.CreateAddItemAsync(addItem);
            return new AddItemDto
            {
                Id = createdAddItem.Id,
                ProductName = createdAddItem.ProductName,
                ItemDescription = createdAddItem.ItemDescription,
                ItemPrice = createdAddItem.ItemPrice,
                Stock = createdAddItem.Stock,
                Size = createdAddItem.Size,
                ImageUrl = createdAddItem.ImageUrl,
                CategoryId = createdAddItem.CategoryId
            };
        }
        public async Task<AddItemDto> UpdateAddItemAsync(int id, UpdateItemDto updateItemDto)
        {
            var existingAddItem = await _addItemRepository.GetAddItemByIdAsync(id);
            if (existingAddItem == null) throw new Exception("AddItem not found");
            existingAddItem.ProductName = updateItemDto.ProductName;
            existingAddItem.ItemDescription = updateItemDto.ItemDescription;
            existingAddItem.ItemPrice = updateItemDto.ItemPrice;
            existingAddItem.Stock = updateItemDto.Stock;
            existingAddItem.Size = updateItemDto.Size;
            existingAddItem.ImageUrl = updateItemDto.ImageUrl;
            existingAddItem.CategoryId = updateItemDto.CategoryId;
            var updatedAddItem = await _addItemRepository.UpdateAddItemAsync(existingAddItem);
            return new AddItemDto
            {
                Id = updatedAddItem.Id,
                ProductName = updatedAddItem.ProductName,
                ItemDescription = updatedAddItem.ItemDescription,
                ItemPrice = updatedAddItem.ItemPrice,
                Stock = updatedAddItem.Stock,
                Size = updatedAddItem.Size,
                ImageUrl = updatedAddItem.ImageUrl,
                CategoryId = updatedAddItem.CategoryId
            };
        }
        public async Task<bool> DeleteAddItemAsync(int id)
        {
            var existingAddItem = await _addItemRepository.GetAddItemByIdAsync(id);
            if (existingAddItem == null) return false;
            await _addItemRepository.DeleteAddItemAsync(id);
            return true;
        }

        public async Task<PagedResponseDTO<AddItemDto>> GetPagedItemsAsync(string? search, int? categoryId, string? size, string? sortBy, int page, int pageSize)
        {
            var (items, totalCount) = await _addItemRepository.GetPagedItemsAsync(search, categoryId, size, sortBy, page, pageSize);

            var dtos = items.Select(ai => new AddItemDto
            {
                Id = ai.Id,
                ProductName = ai.ProductName,
                ItemDescription = ai.ItemDescription,
                ItemPrice = ai.ItemPrice,
                Stock = ai.Stock,
                Size = ai.Size,
                ImageUrl = ai.ImageUrl,
                CategoryId = ai.CategoryId
            }).ToList();

            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            return new PagedResponseDTO<AddItemDto>
            {
                Items = dtos,
                TotalCount = totalCount,
                PageNumber = page,
                TotalPages = totalPages
            };
        }
    }
}
