using BookHub.Core.Entities;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;

namespace BookHub.Core.Interfaces.Repository
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<IEnumerable<Book>> GetBooksWithReviews();
        Task<Book?> GetByIdWithReviews(int id);
        Task<PagedList<Book>> GetPagedBooksWithReviews(GridRequest request);
    }
}