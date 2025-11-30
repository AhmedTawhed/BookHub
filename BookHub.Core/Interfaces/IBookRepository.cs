using BookHub.Core.Entities;

namespace BookHub.Core.Interfaces
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksWithReviews();
    }
}