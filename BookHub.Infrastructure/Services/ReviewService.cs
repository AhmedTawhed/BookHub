using BookHub.Core.DTOs.ReviewDtos;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;
using Microsoft.Extensions.Caching.Memory;

namespace BookHub.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private const string BooksCacheKey = "books_all";

        public ReviewService(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        private ReviewResponseDto MapToDto(Review review)
        {
            return new ReviewResponseDto
            {
                Id = review.Id,
                BookId = review.BookId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            };
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByUser(string userId)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsByUserId(userId);
            return reviews.Select(MapToDto);
        }

        public async Task<IEnumerable<ReviewResponseDto>> GetReviewsByBook(int bookId)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsByBookId(bookId);
            return reviews.Select(MapToDto);
        }

        public async Task<ReviewResponseDto> GetReviewById(int id)
        {
            var review = await _unitOfWork.Reviews.GetById(id);
            if (review == null)
                throw new NotFoundException("Review not found.");
            return MapToDto(review);
        }

        public async Task<ReviewResponseDto> AddReview(ReviewRequestDto dto)
        {
            if (await _unitOfWork.Reviews.IsReviewed(dto.UserId, dto.BookId))
                throw new BadRequestException("You have already reviewed this book.");

            var review = new Review
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.Add(review);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(BooksCacheKey);

            return MapToDto(review);
        }

        public async Task<ReviewResponseDto> UpdateReview(string userId, int reviewId, ReviewRequestDto dto)
        {
            var review = await _unitOfWork.Reviews.GetById(reviewId);

            if (review == null)
                throw new NotFoundException("Review not found.");

            if (review.UserId != userId)
                throw new UnauthorizedException("You can only update your own reviews.");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(BooksCacheKey);

            return MapToDto(review);
        }

        public async Task DeleteReview(string userId, int reviewId)
        {
            var review = await _unitOfWork.Reviews.GetById(reviewId);

            if (review == null)
                throw new NotFoundException("Review not found.");

            if (review.UserId != userId)
                throw new UnauthorizedException("You can only delete your own reviews.");

            _unitOfWork.Reviews.Delete(review);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(BooksCacheKey);
        }
    }
}
