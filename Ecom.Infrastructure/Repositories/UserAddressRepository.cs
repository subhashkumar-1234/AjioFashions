using Microsoft.EntityFrameworkCore;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using Ecom.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Infrastructure.Repositories
{
    public class UserAddressRepository : IUserAddressRepository
    {
        private readonly AppDbContext _context;

        public UserAddressRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<UserAddress>> GetAddressesByUserIdAsync(int userId)
        {
            return await _context.UserAddresses
                .Where(ua => ua.UserId == userId)
                .OrderByDescending(ua => ua.IsDefault)
                .ThenByDescending(ua => ua.CreatedAt)
                .ToListAsync();
        }

        public async Task<UserAddress?> GetAddressByIdAsync(int id)
        {
            return await _context.UserAddresses.FindAsync(id);
        }

        public async Task<UserAddress> AddAddressAsync(UserAddress address)
        {
            if (address.IsDefault)
            {
                await ClearDefaultAddressesAsync(address.UserId);
            }
            _context.UserAddresses.Add(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<UserAddress> UpdateAddressAsync(UserAddress address)
        {
            if (address.IsDefault)
            {
                await ClearDefaultAddressesAsync(address.UserId);
            }
            _context.UserAddresses.Update(address);
            await _context.SaveChangesAsync();
            return address;
        }

        public async Task<bool> DeleteAddressAsync(int id, int userId)
        {
            var address = await _context.UserAddresses.FindAsync(id);
            if (address == null || address.UserId != userId) return false;

            _context.UserAddresses.Remove(address);
            await _context.SaveChangesAsync();
            return true;
        }

        private async Task ClearDefaultAddressesAsync(int userId)
        {
            var defaults = await _context.UserAddresses
                .Where(ua => ua.UserId == userId && ua.IsDefault)
                .ToListAsync();

            foreach (var d in defaults)
            {
                d.IsDefault = false;
            }
        }
    }
}
