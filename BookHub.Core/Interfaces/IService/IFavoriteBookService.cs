using BookHub.Core.DTOs.FavoriteBookDto;

namespace BookHub.Infrastructure.Services
{
    public interface IFavoriteBookService
    {
        Task<FavoriteBookDto> AddFavoriteBook(string userId, int bookId);
        Task<IEnumerable<FavoriteBookDto>> GetUserFavoriteBooks(string userId);
        Task<bool> RemoveFavoriteBook(string userId, int bookId);
    }
}