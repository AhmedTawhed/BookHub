using BookHub.Core.Entities;
using BookHub.Core.Interfaces;
using BookHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Infrastructure.Repositories
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(BookHubDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Review>> GetReviewsByUserId(string userId)
        {
            return await _context.Reviews
                .Where(r => r.UserId == userId)
                .ToListAsync();
        }
        public async Task<IEnumerable<Review>> GetReviewsByBookId(int bookId)
        {
            return await _context.Reviews
                .Where(r => r.BookId == bookId)
                .ToListAsync();
        }

        public async Task<bool> IsReviewed(string userId, int bookId)
        {
            return  await _context.Reviews.AnyAsync(r => r.UserId == userId && r.BookId == bookId);
        }

        public async Task<Review?> GetUserReviewForBook(string userId, int bookId)
        {
            return await _context.Reviews.FirstOrDefaultAsync(r => r.UserId == userId && r.BookId == bookId);
        }
    }
}
