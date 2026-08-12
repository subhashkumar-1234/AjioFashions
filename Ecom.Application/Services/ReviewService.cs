using Ecom.Application.DTOs;
using Ecom.Application.Interfaces;
using Ecom.Application.Interfaces.AllIteam;
using Ecom.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ecom.Application.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IAddItemRepository _addItemRepository;

        public ReviewService(
            IReviewRepository reviewRepository,
            IUserRepository userRepository,
            IAddItemRepository addItemRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _addItemRepository = addItemRepository;
        }

        public async Task<ReviewResponseDTO> CreateReviewAsync(int userId, ReviewCreateDTO reviewDto)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
            {
                throw new Exception("User not found");
            }

            var product = await _addItemRepository.GetAddItemByIdAsync(reviewDto.ProductId);
            if (product == null)
            {
                throw new Exception("Product not found");
            }

            if (reviewDto.Rating < 1 || reviewDto.Rating > 5)
            {
                throw new ArgumentException("Rating must be between 1 and 5 stars");
            }

            var review = new Review
            {
                ProductId = reviewDto.ProductId,
                UserId = userId,
                Rating = reviewDto.Rating,
                Comment = reviewDto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            var createdReview = await _reviewRepository.AddReviewAsync(review);

            return new ReviewResponseDTO
            {
                Id = createdReview.Id,
                ProductId = createdReview.ProductId,
                UserId = createdReview.UserId,
                UserName = user.Name,
                Rating = createdReview.Rating,
                Comment = createdReview.Comment,
                CreatedAt = createdReview.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewResponseDTO>> GetReviewsByProductIdAsync(int productId)
        {
            var reviews = await _reviewRepository.GetReviewsByProductIdAsync(productId);
            return reviews.Select(r => new ReviewResponseDTO
            {
                Id = r.Id,
                ProductId = r.ProductId,
                UserId = r.UserId,
                UserName = r.User?.Name ?? "Anonymous User",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            });
        }
    }
}
