using BookHub.Core.DTOs.ReviewDtos;

namespace BookHub.Core.Interfaces.Service
{
    public interface IReviewService
    {
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByUser(string userId);
        Task<IEnumerable<ReviewResponseDto>> GetReviewsByBook(int bookId);
        Task<ReviewResponseDto> AddReview(ReviewRequestDto dto);
        Task<ReviewResponseDto> UpdateReview(string userId, int reviewId, ReviewRequestDto dto);
    }
}