using BookHub.Core.Entities;

namespace BookHub.Core.Interfaces
{
    public interface IFavoriteBookRepository : IRepository<FavoriteBook>
    {
        Task<FavoriteBook?> GetFavoriteBook(string userId, int bookId);
        Task<IEnumerable<FavoriteBook>> GetFavoriteBooksByUserId(string userId);
    }
}