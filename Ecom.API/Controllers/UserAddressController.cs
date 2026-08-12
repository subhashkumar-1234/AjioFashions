using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Ecom.Application.Interfaces;
using Ecom.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserAddressController : ControllerBase
    {
        private readonly IUserAddressService _addressService;

        public UserAddressController(IUserAddressService addressService)
        {
            _addressService = addressService;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !int.TryParse(userIdClaim, out int userId))
            {
                throw new System.UnauthorizedAccessException();
            }
            return userId;
        }

        [HttpGet]
        public async Task<IActionResult> GetAddresses()
        {
            try
            {
                var userId = GetUserId();
                var addresses = await _addressService.GetAddressesByUserIdAsync(userId);
                return Ok(addresses);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAddress(int id)
        {
            try
            {
                var userId = GetUserId();
                var address = await _addressService.GetAddressByIdAsync(id);
                if (address == null || address.UserId != userId)
                {
                    return NotFound("Address not found.");
                }
                return Ok(address);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress([FromBody] AddressCreateDTO addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserId();
                var created = await _addressService.AddAddressAsync(userId, addressDto);
                return Ok(created);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAddress(int id, [FromBody] AddressCreateDTO addressDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var userId = GetUserId();
                var updated = await _addressService.UpdateAddressAsync(userId, id, addressDto);
                return Ok(updated);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
            catch (System.Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            try
            {
                var userId = GetUserId();
                var success = await _addressService.DeleteAddressAsync(id, userId);
                if (!success)
                {
                    return NotFound("Address not found or unauthorized.");
                }
                return Ok(new { message = "Address deleted successfully." });
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
