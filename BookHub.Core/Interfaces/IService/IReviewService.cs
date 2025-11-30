using BookHub.Core.DTOs.ReviewDto;

namespace BookHub.Infrastructure.Services
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewDto>> GetReviewsByUser(string userId);
        Task<IEnumerable<ReviewDto>> GetReviewsByBook(int bookId);
        Task AddReview(string userId, int bookId, ReviewResponseDto dto);
        Task UpdateReview(string userId, int reviewId, ReviewResponseDto dto);
    }
}