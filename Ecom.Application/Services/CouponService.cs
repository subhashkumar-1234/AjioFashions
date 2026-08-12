using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace Ecom.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _couponRepository;

        public CouponService(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
        }

        public async Task<CouponDTO?> GetCouponByCodeAsync(string code)
        {
            var coupon = await _couponRepository.GetCouponByCodeAsync(code);
            if (coupon == null) return null;
            return MapToDTO(coupon);
        }

        public async Task<CouponDTO> CreateCouponAsync(CouponDTO couponDto)
        {
            var coupon = new Coupon
            {
                Code = couponDto.Code.ToUpper(),
                DiscountPercentage = couponDto.DiscountPercentage,
                ExpiryDate = couponDto.ExpiryDate,
                IsActive = couponDto.IsActive,
                SellerId = couponDto.SellerId
            };
            var created = await _couponRepository.CreateCouponAsync(coupon);
            return MapToDTO(created);
        }

        public async Task<CouponDTO?> ValidateCouponAsync(string code)
        {
            var coupon = await _couponRepository.GetCouponByCodeAsync(code);
            if (coupon == null) return null;

            if (!coupon.IsActive || coupon.ExpiryDate < DateTime.UtcNow)
            {
                return null;
            }

            return MapToDTO(coupon);
        }

        private CouponDTO MapToDTO(Coupon coupon)
        {
            return new CouponDTO
            {
                Id = coupon.Id,
                Code = coupon.Code,
                DiscountPercentage = coupon.DiscountPercentage,
                ExpiryDate = coupon.ExpiryDate,
                IsActive = coupon.IsActive,
                SellerId = coupon.SellerId
            };
        }
    }
}
