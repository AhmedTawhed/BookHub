using BookHub.Core.Entities;

namespace BookHub.Core.Interfaces.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksWithReviews();
    }
}