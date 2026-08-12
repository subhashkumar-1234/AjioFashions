using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;

using System;
using System.Collections.Generic;
using System.Text;

namespace Ecom.Application.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;
        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<IEnumerable<RoleCreateDTO>> GetAllRolesAsync()
        {
            var roles = await _roleRepository.GetAllRolesAsync();
            return roles.Select(r => new RoleCreateDTO
            {
                Id = r.Id,
                RoleName = r.RoleName
            });
        }
        public async Task<RoleCreateDTO?> GetRoleByIdAsync(int id)
        {
            var role = await _roleRepository.GetRoleByIdAsync(id);
            if (role == null) return null;
            return new RoleCreateDTO
            {
                Id = role.Id,
                RoleName = role.RoleName
            };
        }
        public async Task<RoleCreateDTO> CreateRoleAsync(RoleCreateDTO roleDto)
        {
            var role = new Role
            {
                RoleName = roleDto.RoleName
            };
            var createdRole = await _roleRepository.AddRoleAsync(role);
            return new RoleCreateDTO
            {
                Id = createdRole.Id,
                RoleName = createdRole.RoleName
            };
        }
        public async Task<RoleCreateDTO> UpdateRoleAsync(int id, RoleCreateDTO roleDto)
        {
            var existingRole = await _roleRepository.GetRoleByIdAsync(id);
            if (existingRole == null) throw new Exception("Role not found");
            existingRole.RoleName = roleDto.RoleName;
            var updatedRole = await _roleRepository.UpdateRoleAsync(existingRole);
            return new RoleCreateDTO
            {
                Id = updatedRole.Id,
                RoleName = updatedRole.RoleName
            };
        }
        public async Task<bool> DeleteRoleAsync(int id)
        {
            return await _roleRepository.DeleteRoleAsync(id);
        }
}
}
