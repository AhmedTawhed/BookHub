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
    public class FavoriteBookRepository : Repository<FavoriteBook>, IFavoriteBookRepository
    {
        public FavoriteBookRepository(BookHubDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<FavoriteBook>> GetFavoriteBooksByUserId(string userId)
        {
            return await _context.FavoriteBooks
                .Where(b => b.UserId == userId)
                .ToListAsync();
        }

        public async Task<FavoriteBook?> GetFavoriteBook(string userId, int bookId)
        {
            return await _context.FavoriteBooks
                .FirstOrDefaultAsync(b => b.UserId == userId && b.BookId == bookId);
        }
    }
}
