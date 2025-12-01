using BookHub.Core.Entities;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BookHub.Infrastructure.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(BookHubDbContext context) : base(context)
        {
        }
        public async Task<IEnumerable<Book>> GetBooksWithReviews()
        {
            return await _context.Books
                .Include(b => b.Reviews)
                .Include(b => b.Category)
                .ToListAsync();
        }
    }
}
