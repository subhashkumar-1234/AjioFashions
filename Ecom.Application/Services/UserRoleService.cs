using Ecom.Application.Interfaces;
using Ecom.Application.DTOs;
using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public async Task<IEnumerable<UserRoleCreateDTO>> GetAllUserRolesAsync()
        {
            var userRoles = await _userRoleRepository.GetAllUserRolesAsync();
            return userRoles.Select(ur => new UserRoleCreateDTO
            {
                Id = ur.Id,
                UserId = ur.UserId,
                RoleId = ur.RoleId
            });
        }
        public async Task<UserRoleCreateDTO?> GetUserRoleByIdAsync(int id)
        {
            var userRole = await _userRoleRepository.GetUserRoleByIdAsync(id);
            if (userRole == null) return null;
            return new UserRoleCreateDTO
            {
                Id = userRole.Id,
                UserId = userRole.UserId,
                RoleId = userRole.RoleId
            };
        }
        public async Task<UserRoleCreateDTO> CreateUserRoleAsync(UserRoleCreateDTO userRoleDto)
        {
            var userRole = new UserRole
            {
                UserId = userRoleDto.UserId,
                RoleId = userRoleDto.RoleId
            };
            var createdUserRole = await _userRoleRepository.AddUserRoleAsync(userRole);
            return new UserRoleCreateDTO
            {
                Id = createdUserRole.Id,
                UserId = createdUserRole.UserId,
                RoleId = createdUserRole.RoleId
            };
        }
        public async Task<UserRoleCreateDTO> UpdateUserRoleAsync(int id, UserRoleCreateDTO userRoleDto)
        {
            var existingUserRole = await _userRoleRepository.GetUserRoleByIdAsync(id);
            if (existingUserRole == null) throw new Exception("UserRole not found");
            existingUserRole.UserId = userRoleDto.UserId;
            existingUserRole.RoleId = userRoleDto.RoleId;
            var updatedUserRole = await _userRoleRepository.UpdateUserRoleAsync(existingUserRole);
            return new UserRoleCreateDTO
            {
                Id = updatedUserRole.Id,
                UserId = updatedUserRole.UserId,
                RoleId = updatedUserRole.RoleId
            };
        }
        public async Task<bool> DeleteUserRoleAsync(int id)
        {
            return await _userRoleRepository.DeleteUserRoleAsync(id);
        }
    }
}
