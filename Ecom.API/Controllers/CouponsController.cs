using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Ecom.Application.Interfaces;
using Ecom.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Ecom.Infrastructure.Data;
using System;

namespace Ecom.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponsController : ControllerBase
    {
        private readonly ICouponService _couponService;

        public CouponsController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        [HttpGet("validate/{code}")]
        public async Task<IActionResult> ValidateCoupon(string code)
        {
            var coupon = await _couponService.ValidateCouponAsync(code);
            if (coupon == null)
            {
                return BadRequest("Invalid, expired, or inactive coupon code.");
            }
            return Ok(coupon);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> CreateCoupon([FromBody] CouponDTO couponDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (User.IsInRole("Seller"))
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (int.TryParse(userIdClaim, out int sellerId))
                {
                    couponDto.SellerId = sellerId;
                }
                else
                {
                    return Unauthorized("Invalid user identification claim");
                }
            }

            var created = await _couponService.CreateCouponAsync(couponDto);
            return Ok(created);
        }

        [HttpGet("seller-owned")]
        [Authorize(Roles = "Seller")]
        public async Task<IActionResult> GetSellerOwnedCoupons([FromServices] AppDbContext context)
        {
            try
            {
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(userIdClaim, out int sellerId))
                {
                    return Unauthorized("Invalid user identification claim");
                }

                var coupons = await context.Coupons
                    .Where(c => c.SellerId == sellerId)
                    .ToListAsync();
                return Ok(coupons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllCoupons([FromServices] AppDbContext context)
        {
            try
            {
                var coupons = await context.Coupons.ToListAsync();
                return Ok(coupons);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,Seller")]
        public async Task<IActionResult> DeleteCoupon(int id, [FromServices] AppDbContext context)
        {
            try
            {
                var coupon = await context.Coupons.FindAsync(id);
                if (coupon == null) return NotFound("Coupon not found");

                if (User.IsInRole("Seller"))
                {
                    var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                    if (!int.TryParse(userIdClaim, out int sellerId) || coupon.SellerId != sellerId)
                    {
                        return Forbid();
                    }
                }

                context.Coupons.Remove(coupon);
                await context.SaveChangesAsync();
                return Ok(new { Message = "Coupon deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}
