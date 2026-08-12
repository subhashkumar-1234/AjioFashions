using Ecom.Application.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecom.Application.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO> CreateReviewAsync(int userId, ReviewCreateDTO reviewDto);
        Task<IEnumerable<ReviewResponseDTO>> GetReviewsByProductIdAsync(int productId);
    }
}
