using Ecom.Application.DTOs;

namespace Ecom.Application.Interfaces
{
    public interface IUserRoleService
    {
        Task<IEnumerable<UserRoleCreateDTO>> GetAllUserRolesAsync();
        Task<UserRoleCreateDTO?> GetUserRoleByIdAsync(int id);
        Task<UserRoleCreateDTO> CreateUserRoleAsync(UserRoleCreateDTO userRoleDto);
        Task<UserRoleCreateDTO> UpdateUserRoleAsync(int id, UserRoleCreateDTO userRoleDto);
        Task<bool> DeleteUserRoleAsync(int id);
    }
}
