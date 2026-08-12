using System;
using System.Collections.Generic;
using System.Text;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Application.DTOs.AllItemDtos;
using Ecom.Domain.Entities;
namespace Ecom.Application.Services
{
    public class AddQuantityService : IAddQuantityService
    {
        private readonly IAddQuantityRepository _addQuantityRepository;
        public AddQuantityService(IAddQuantityRepository addQuantityRepository)
        {
            _addQuantityRepository = addQuantityRepository;
        }
        public async Task<IEnumerable<AddQuantityDto>> GetAllAddQuantitiesAsync()
        {
            var addQuantities = await _addQuantityRepository.GetAllAddQuantitiesAsync();
            return addQuantities.Select(aq => new AddQuantityDto
            {
                Id = aq.Id,
                AddItemId = aq.AddItemId,
                Quantity = aq.Quantity,
                PreviousStock = aq.PreviousStock,
                CurrentStock = aq.CurrentStock,
                CreatedAt = aq.CreatedAt,
                UpdatedAt = aq.UpdatedAt
            });
        }
        public async Task<AddQuantityDto?> GetAddQuantityByIdAsync(int id)
        {
            var addQuantity = await _addQuantityRepository.GetAddQuantityByIdAsync(id);
            if (addQuantity == null) return null;
            return new AddQuantityDto
            {
                Id = addQuantity.Id,
                AddItemId = addQuantity.AddItemId,
                Quantity = addQuantity.Quantity,
                PreviousStock = addQuantity.PreviousStock,
                CurrentStock = addQuantity.CurrentStock,
                CreatedAt = addQuantity.CreatedAt,
                UpdatedAt = addQuantity.UpdatedAt
            };
        }
        public async Task<AddQuantityDto> CreateAddQuantityAsync(AddQuantityDto addQuantityDto)
        {
            var addQuantity = new AddQuantity
            {
                AddItemId = addQuantityDto.AddItemId,
                Quantity = addQuantityDto.Quantity,
                PreviousStock = addQuantityDto.PreviousStock,
                CurrentStock = addQuantityDto.CurrentStock
            };
            var createdAddQuantity = await _addQuantityRepository.CreateAddQuantityAsync(addQuantity);
            return new AddQuantityDto
            {
                Id = createdAddQuantity.Id,
                AddItemId = createdAddQuantity.AddItemId,
                Quantity = createdAddQuantity.Quantity,
                PreviousStock = createdAddQuantity.PreviousStock,
                CurrentStock = createdAddQuantity.CurrentStock,
                CreatedAt = createdAddQuantity.CreatedAt,
                UpdatedAt = createdAddQuantity.UpdatedAt
            };
        }
        public async Task<AddQuantityDto?> UpdateAddQuantityAsync(int id, UpdateQuantityDto updateQuantityDto)
        {
            var existingAddQuantity = await _addQuantityRepository.GetAddQuantityByIdAsync(id);
            if (existingAddQuantity == null) return null;
            existingAddQuantity.AddItemId = updateQuantityDto.AddItemId;
            existingAddQuantity.Quantity = updateQuantityDto.Quantity;
            existingAddQuantity.PreviousStock = updateQuantityDto.PreviousStock;
            existingAddQuantity.CurrentStock = updateQuantityDto.CurrentStock;
            var updatedAddQuantity = await _addQuantityRepository.UpdateAddQuantityAsync(existingAddQuantity);
            return new AddQuantityDto
            {
                Id = updatedAddQuantity.Id,
                AddItemId = updatedAddQuantity.AddItemId,
                Quantity = updatedAddQuantity.Quantity,
                PreviousStock = updatedAddQuantity.PreviousStock,
                CurrentStock = updatedAddQuantity.CurrentStock,
                CreatedAt = updatedAddQuantity.CreatedAt,
                UpdatedAt = updatedAddQuantity.UpdatedAt
            };
        }
        public async Task<bool> DeleteAddQuantityAsync(int id)
        {
            return await _addQuantityRepository.DeleteAddQuantityAsync(id);
        }
}
}
