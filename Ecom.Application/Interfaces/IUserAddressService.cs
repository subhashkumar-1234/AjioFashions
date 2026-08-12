using Ecom.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IUserAddressService
    {
        Task<IEnumerable<AddressDTO>> GetAddressesByUserIdAsync(int userId);
        Task<AddressDTO?> GetAddressByIdAsync(int id);
        Task<AddressDTO> AddAddressAsync(int userId, AddressCreateDTO addressDto);
        Task<AddressDTO> UpdateAddressAsync(int userId, int id, AddressCreateDTO addressDto);
        Task<bool> DeleteAddressAsync(int id, int userId);
    }
}
