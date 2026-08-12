using Ecom.Domain.Entities;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetCouponByCodeAsync(string code);
        Task<Coupon> CreateCouponAsync(Coupon coupon);
    }
}
