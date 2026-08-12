using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Security.Claims;
using Ecom.Application.Interfaces;
using Ecom.Application.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
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
        public async Task<IActionResult> GetCart()
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.GetCartByUserIdAsync(userId);
                return Ok(cart);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncCart([FromBody] CartSyncDTO syncDto)
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.SyncCartAsync(userId, syncDto);
                return Ok(cart);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("item")]
        public async Task<IActionResult> AddOrUpdateItem([FromBody] CartItemUpdateDTO itemUpdate)
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.AddOrUpdateCartItemAsync(userId, itemUpdate);
                return Ok(cart);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("item/{productId}")]
        public async Task<IActionResult> RemoveItem(int productId)
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.RemoveCartItemAsync(userId, productId);
                return Ok(cart);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var userId = GetUserId();
                var cart = await _cartService.ClearCartAsync(userId);
                return Ok(cart);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }
}
