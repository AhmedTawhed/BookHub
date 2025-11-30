using BookHub.Core.Entities;

namespace BookHub.Infrastructure.Repositories
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksWithReviews();
    }
}