using BookHub.Core.Entities;
using BookHub.Core.Interfaces;
using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
