using BookHub.Core.Entities;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

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

        public async Task<Book?> GetByIdWithReviews(int id)
        {
            return await _context.Books
                .Include(b => b.Reviews)
                .Include(b => b.Category)
                .FirstOrDefaultAsync(b => b.Id == id);
        }

        public async Task<PagedList<Book>> GetPagedBooksWithReviews(GridRequest request)
        {
            var query = _context.Books
                .Include(b => b.Reviews)
                .Include(b => b.Category)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var text = request.SearchText.Trim().ToLower();
                query = query.Where(b => b.Title.ToLower().Contains(text) || b.Author.ToLower().Contains(text));
            }

            if (!string.IsNullOrEmpty(request.SortColumn))
            {
                var direction = request.SortDirection?.ToLower() == "desc" ? "desc" : "asc";
                query = query.OrderBy($"{request.SortColumn} {direction}");
            }

            int skip = request.Skip < 0 ? 0 : request.Skip;
            int take = request.Take <= 0 ? 10 : Math.Min(request.Take, 100);

            var totalCount = await query.CountAsync();
            var items = await query.Skip(skip).Take(take).ToListAsync();

            return new PagedList<Book>
            {
                Items = items,
                TotalCount = totalCount,
                NumberOfPages = (int)Math.Ceiling((double)totalCount / take)
            };
        }
    }
}
