using BookHub.Core.DTOs.ReviewDto;
using BookHub.Core.Entities;
using BookHub.Core.Interfaces;

namespace BookHub.Infrastructure.Services
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReviewService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ReviewDto>> GetReviewsByUser(string userId)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsByUserId(userId);
            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAtUtc
            });
        }
        public async Task<IEnumerable<ReviewDto>> GetReviewsByBook(int bookId)
        {
            var reviews = await _unitOfWork.Reviews.GetReviewsByBookId(bookId);
            if (reviews == null)
                return Enumerable.Empty<ReviewDto>();

            return reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                BookId = r.BookId,
                UserId = r.UserId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAtUtc
            });
        }

        public async Task<ReviewDto> AddReview(string userId, int bookId, ReviewResponseDto dto)
        {
            var reviewed = await _unitOfWork.Reviews.IsReviewed(userId, bookId);

            if (reviewed)
                throw new Exception("Book already reviewed.");

            var review = new Review
            {
                UserId = userId,
                BookId = bookId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAtUtc = DateTime.UtcNow
            };

            await _unitOfWork.Reviews.Add(review);
            await _unitOfWork.CompleteAsync();

            return new ReviewDto
            {
                Id = review.Id,
                BookId = review.BookId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAtUtc
            };
        }

        public async Task<ReviewDto> UpdateReview(string userId, int reviewId, ReviewResponseDto dto)
        {
            var review = await _unitOfWork.Reviews.GetById(reviewId);

            if (review == null)
                throw new Exception("Review not found.");

            if (review.UserId != userId)
                throw new UnauthorizedAccessException();

            review.Rating = dto.Rating;
            review.Comment = dto.Comment;

            _unitOfWork.Reviews.Update(review);
            await _unitOfWork.CompleteAsync();

            return new ReviewDto
            {
                Id = review.Id,
                BookId = review.BookId,
                UserId = review.UserId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAtUtc
            };
        }
    }
}
