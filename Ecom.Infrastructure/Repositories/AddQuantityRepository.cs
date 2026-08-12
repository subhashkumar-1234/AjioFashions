using System;
using System.Collections.Generic;
using System.Text;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
namespace Ecom.Infrastructure.Repositories
{
    public class AddQuantityRepository : IAddQuantityRepository
    {
        public readonly AppDbContext _context;
        public AddQuantityRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<AddQuantity>> GetAllAddQuantitiesAsync()
        {
            return await _context.AddQuantities.ToListAsync();
        }
        public async Task<AddQuantity?> GetAddQuantityByIdAsync(int id)
        {
            return await _context.AddQuantities.FindAsync(id);
        }
        public async Task<AddQuantity> CreateAddQuantityAsync(AddQuantity addQuantity)
        {
            _context.AddQuantities.Add(addQuantity);
            await _context.SaveChangesAsync();
            return addQuantity;
        }
        public async Task<AddQuantity> UpdateAddQuantityAsync(AddQuantity addQuantity)
        {
            _context.AddQuantities.Update(addQuantity);
            await _context.SaveChangesAsync();
            return addQuantity;
        }
        public async Task<bool> DeleteAddQuantityAsync(int id)
        {
            var addQuantity = await _context.AddQuantities.FindAsync(id);
            if (addQuantity == null) return false;
            _context.AddQuantities.Remove(addQuantity);
            await _context.SaveChangesAsync();
            return true;
        }
}
}
