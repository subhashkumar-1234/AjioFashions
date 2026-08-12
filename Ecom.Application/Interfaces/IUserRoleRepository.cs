using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Interfaces
{
    public interface IUserRoleRepository
    {
        Task<IEnumerable<UserRole>> GetAllUserRolesAsync();
        Task<UserRole?> GetUserRoleByIdAsync(int id);

        Task<UserRole> AddUserRoleAsync(UserRole role);
        Task<UserRole> UpdateUserRoleAsync(UserRole role);
        Task<bool> DeleteUserRoleAsync(int id);
    }
}
