using BookHub.Core.Entities;

namespace BookHub.Infrastructure.Repositories
{
    public interface IFavoriteBookRepository : IRepository<FavoriteBook>
    {
        Task<FavoriteBook?> GetFavoriteBook(string userId, int bookId);
        Task<IEnumerable<FavoriteBook>> GetFavoriteBooksByUserId(string userId);
    }
}