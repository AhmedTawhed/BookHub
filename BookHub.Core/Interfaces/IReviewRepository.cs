using BookHub.Core.Entities;

namespace BookHub.Core.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByUserId(string userId);
        Task<bool> IsReviewed(string userId, int bookId);
        Task<IEnumerable<Review>> GetReviewsByBookId(int bookId);
        Task<Review?> GetUserReviewForBook(string userId, int bookId);
    }
}