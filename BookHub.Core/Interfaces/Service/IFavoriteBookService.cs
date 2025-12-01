using BookHub.Core.DTOs.FavoriteBookDtos;

namespace BookHub.Core.Interfaces.Service
{
    public interface IFavoriteBookService
    {
        Task<FavoriteBookDto> AddFavoriteBook(string userId, int bookId);
        Task<IEnumerable<FavoriteBookDto>> GetUserFavoriteBooks(string userId);
        Task<bool> RemoveFavoriteBook(string userId, int bookId);
    }
}