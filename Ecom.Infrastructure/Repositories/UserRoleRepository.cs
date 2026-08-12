using Ecom.Domain.Entities;
using Ecom.Application.Interfaces;
using Ecom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Infrastructure.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly AppDbContext _context;
        public UserRoleRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<UserRole>> GetAllUserRolesAsync()
        {
            return await _context.UserRoles.ToListAsync();
        }
        public async Task<UserRole?> GetUserRoleByIdAsync(int id)
        {
            return await _context.UserRoles.FindAsync(id);
        }
        public async Task<UserRole> AddUserRoleAsync(UserRole role)
        {
            _context.UserRoles.Add(role);
            await _context.SaveChangesAsync();
            return role;
        }
        public async Task<UserRole> UpdateUserRoleAsync(UserRole role)
        {
            _context.UserRoles.Update(role);
            await _context.SaveChangesAsync();
            return role;
        }
        public async Task<bool> DeleteUserRoleAsync(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);
            if (userRole == null) return false;
            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
            return true;
        }
}
}
