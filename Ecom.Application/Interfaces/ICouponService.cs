using Ecom.Application.DTOs;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface ICouponService
    {
        Task<CouponDTO?> GetCouponByCodeAsync(string code);
        Task<CouponDTO> CreateCouponAsync(CouponDTO couponDto);
        Task<CouponDTO?> ValidateCouponAsync(string code);
    }
}
