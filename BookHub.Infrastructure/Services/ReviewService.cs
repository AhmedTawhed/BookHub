using BookHub.Core.DTOs.ReviewDtos;
using BookHub.Core.Entities;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;

namespace BookHub.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                CreatedAt = review.CreatedAtUtc
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
            if (reviews == null)
                return Enumerable.Empty<ReviewResponseDto>();

            return reviews.Select(MapToDto);

        }

        public async Task<ReviewResponseDto> AddReview(ReviewRequestDto dto)
        {
            if (await _unitOfWork.Reviews.IsReviewed(dto.UserId, dto.BookId))
                throw new InvalidOperationException("Book has already been reviewed.");

            var review = new Review
            {
                UserId = dto.UserId,
                BookId = dto.BookId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.Add(review);
            await _unitOfWork.CompleteAsync();

            return MapToDto(review);

        }

        public async Task<ReviewResponseDto> UpdateReview(string userId, int reviewId, ReviewRequestDto dto)
        {
            var review = await _unitOfWork.Reviews.GetById(reviewId);

            if (review == null)
                throw new KeyNotFoundException("Review not found.");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException("You can only update your own reviews.");

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.CompleteAsync();

            return MapToDto(review);
        }
    }
}
