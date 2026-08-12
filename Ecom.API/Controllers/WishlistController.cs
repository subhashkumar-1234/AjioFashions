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
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;

        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
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
        public async Task<IActionResult> GetWishlist()
        {
            try
            {
                var userId = GetUserId();
                var wishlist = await _wishlistService.GetWishlistByUserIdAsync(userId);
                return Ok(wishlist);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("sync")]
        public async Task<IActionResult> SyncWishlist([FromBody] WishlistSyncDto syncDto)
        {
            try
            {
                var userId = GetUserId();
                var wishlist = await _wishlistService.SyncWishlistAsync(userId, syncDto);
                return Ok(wishlist);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("item")]
        public async Task<IActionResult> AddItem([FromBody] WishlistAddItemRequest request)
        {
            try
            {
                var userId = GetUserId();
                var wishlist = await _wishlistService.AddToWishlistAsync(userId, request.ProductId);
                return Ok(wishlist);
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
                var wishlist = await _wishlistService.RemoveFromWishlistAsync(userId, productId);
                return Ok(wishlist);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete]
        public async Task<IActionResult> ClearWishlist()
        {
            try
            {
                var userId = GetUserId();
                var wishlist = await _wishlistService.ClearWishlistAsync(userId);
                return Ok(wishlist);
            }
            catch (System.UnauthorizedAccessException)
            {
                return Unauthorized();
            }
        }
    }

    public class WishlistAddItemRequest
    {
        public int ProductId { get; set; }
    }
}
