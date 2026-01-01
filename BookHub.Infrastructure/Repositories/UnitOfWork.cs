using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Data;

namespace BookHub.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BookHubDbContext _context;
        IBookRepository _bookRepository = default!;
        ICategoryRepository _categoryRepository = default!;
        IReviewRepository _reviewRepository = default!;
        IFavoriteBookRepository _favoriteBookRepository = default!;

        public UnitOfWork(BookHubDbContext context)
        {
            _context = context;
        }

        public IBookRepository Books => _bookRepository ??= new BookRepository(_context);
        public ICategoryRepository Categories => _categoryRepository ??= new CategoryRepository(_context);
        public IReviewRepository Reviews => _reviewRepository ??= new ReviewRepository(_context);
        public IFavoriteBookRepository FavoriteBooks => _favoriteBookRepository ??= new FavoriteBookRepository(_context);

        public async Task<int> CompleteAsync() => await _context.SaveChangesAsync();
        public void Dispose() => _context.Dispose();
    }
}
