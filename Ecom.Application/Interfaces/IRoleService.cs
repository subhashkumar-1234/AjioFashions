using Ecom.Application.DTOs;

namespace Ecom.Application.Interfaces
{
    public interface IRoleService
    {
        Task<IEnumerable<RoleCreateDTO>> GetAllRolesAsync();
        Task<RoleCreateDTO?> GetRoleByIdAsync(int id);
        Task<RoleCreateDTO> CreateRoleAsync(RoleCreateDTO roleDto);
        Task<RoleCreateDTO> UpdateRoleAsync(int id, RoleCreateDTO roleDto);
        Task<bool> DeleteRoleAsync(int id);
    }
}
