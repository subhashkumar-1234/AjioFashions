using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Interfaces.AllIteam
{
    public interface IAddQuantityRepository
    {
        public Task<IEnumerable<AddQuantity>> GetAllAddQuantitiesAsync();
        public Task<AddQuantity?> GetAddQuantityByIdAsync(int id);
        public Task<AddQuantity> CreateAddQuantityAsync(AddQuantity addQuantity);
        public Task<AddQuantity> UpdateAddQuantityAsync(AddQuantity addQuantity);
        public Task<bool> DeleteAddQuantityAsync(int id);
    }
}
