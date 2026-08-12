using Ecom.Application.DTOs.AllItemDtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Interfaces.AllIteam
{
    public interface IAddQuantityService
    {
        public Task<IEnumerable<AddQuantityDto>> GetAllAddQuantitiesAsync();
        public Task<AddQuantityDto?> GetAddQuantityByIdAsync(int id);
        public Task<AddQuantityDto> CreateAddQuantityAsync(AddQuantityDto addQuantityDto);
        public Task<AddQuantityDto> UpdateAddQuantityAsync(int id, UpdateQuantityDto updateQuantityDto);
        public Task<bool> DeleteAddQuantityAsync(int id);
    }
}
