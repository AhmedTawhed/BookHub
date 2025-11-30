using BookHub.Core.Entities;

namespace BookHub.Infrastructure.Repositories
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetReviewsByUserId(string userId);
        bool IsReviewd(string userId, int bookId);
        Task<IEnumerable<Review>> GetReviewsByBookId(int bookId);
        Task<Review?> GetUserReviewForBook(string userId, int bookId);
    }
}