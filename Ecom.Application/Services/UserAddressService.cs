using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Application.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _addressRepository;

        public UserAddressService(IUserAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public async Task<IEnumerable<AddressDTO>> GetAddressesByUserIdAsync(int userId)
        {
            var addresses = await _addressRepository.GetAddressesByUserIdAsync(userId);
            return addresses.Select(MapToDTO);
        }

        public async Task<AddressDTO?> GetAddressByIdAsync(int id)
        {
            var address = await _addressRepository.GetAddressByIdAsync(id);
            if (address == null) return null;
            return MapToDTO(address);
        }

        public async Task<AddressDTO> AddAddressAsync(int userId, AddressCreateDTO addressDto)
        {
            var address = new UserAddress
            {
                UserId = userId,
                AddressLine = addressDto.AddressLine,
                PhoneNumber = addressDto.PhoneNumber,
                PostalCode = addressDto.PostalCode,
                IsDefault = addressDto.IsDefault,
                CreatedAt = DateTime.UtcNow
            };
            var created = await _addressRepository.AddAddressAsync(address);
            return MapToDTO(created);
        }

        public async Task<AddressDTO> UpdateAddressAsync(int userId, int id, AddressCreateDTO addressDto)
        {
            var existing = await _addressRepository.GetAddressByIdAsync(id);
            if (existing == null || existing.UserId != userId)
            {
                throw new Exception("Address not found or unauthorized");
            }

            existing.AddressLine = addressDto.AddressLine;
            existing.PhoneNumber = addressDto.PhoneNumber;
            existing.PostalCode = addressDto.PostalCode;
            existing.IsDefault = addressDto.IsDefault;

            var updated = await _addressRepository.UpdateAddressAsync(existing);
            return MapToDTO(updated);
        }

        public async Task<bool> DeleteAddressAsync(int id, int userId)
        {
            return await _addressRepository.DeleteAddressAsync(id, userId);
        }

        private AddressDTO MapToDTO(UserAddress address)
        {
            return new AddressDTO
            {
                Id = address.Id,
                UserId = address.UserId,
                AddressLine = address.AddressLine,
                PhoneNumber = address.PhoneNumber,
                PostalCode = address.PostalCode,
                IsDefault = address.IsDefault
            };
        }
    }
}
