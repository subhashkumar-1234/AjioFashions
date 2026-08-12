using Ecom.Application.DTOs;
using Ecom.Domain.Entities;
namespace Ecom.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserCreateDTO>> GetAllUsersAsync();
        Task<UserCreateDTO?> GetUserByIdAsync(int id);
        Task<UserCreateDTO> CreateUserAsync(UserCreateDTO userDto);
        Task<UserCreateDTO> UpdateUserAsync(int id, UserUpdateDTO userDto);
        Task<bool> DeleteUserAsync(int id);
        
    }
}
