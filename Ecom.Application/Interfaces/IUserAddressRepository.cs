using Ecom.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IUserAddressRepository
    {
        Task<IEnumerable<UserAddress>> GetAddressesByUserIdAsync(int userId);
        Task<UserAddress?> GetAddressByIdAsync(int id);
        Task<UserAddress> AddAddressAsync(UserAddress address);
        Task<UserAddress> UpdateAddressAsync(UserAddress address);
        Task<bool> DeleteAddressAsync(int id, int userId);
    }
}
