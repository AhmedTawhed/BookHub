using BookHub.Core.DTOs.FavoriteBookDto;

namespace BookHub.Infrastructure.Services
{
    public interface IFavoriteBookService
    {
        Task AddFavoriteBook(string userId, FavoriteBookResponseDto dto);
        Task<IEnumerable<FavoriteBookDto>> GetUserFavoriteBooks(string userId);
        Task RemoveFavoriteBook(string userId, int bookId);
    }
}