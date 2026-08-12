using Ecom.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IReviewRepository
    {
        Task<IEnumerable<Review>> GetReviewsByProductIdAsync(int productId);
        Task<Review> AddReviewAsync(Review review);
    }
}
