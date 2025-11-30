using BookHub.Core.DTOs.FavoriteBookDto;
using BookHub.Core.Entities;
using BookHub.Core.Interfaces;

namespace BookHub.Infrastructure.Services
{
    public class FavoriteBookService : IFavoriteBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteBookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<FavoriteBookDto>> GetUserFavoriteBooks(string userId)
        {
            var favoriteBooks = await _unitOfWork.FavoriteBooks.GetFavoriteBooksByUserId(userId);
            return favoriteBooks.Select(b => new FavoriteBookDto
            {
                BookId = b.BookId,
                BookTitle = b.Book.Title,
                UserId = b.UserId
            });
        }

        public async Task<FavoriteBookDto> AddFavoriteBook(string userId, int bookId)
        {
            var exists = await _unitOfWork.FavoriteBooks.GetFavoriteBook(userId, bookId);

            if (exists != null)
                throw new Exception("Book already in favorites.");

            var book = await _unitOfWork.Books.GetById(bookId);
            if (book == null)
                throw new Exception("Book does not exist.");

            var favorite = new FavoriteBook
            {
                UserId = userId,
                BookId = bookId
            };

            await _unitOfWork.FavoriteBooks.Add(favorite);
            await _unitOfWork.CompleteAsync();

            return new FavoriteBookDto
            {
                BookId = favorite.BookId,
                BookTitle = favorite.Book.Title,
                UserId = favorite.UserId
            };
        }

        public async Task<bool> RemoveFavoriteBook(string userId, int bookId)
        {
            var favorite = await _unitOfWork.FavoriteBooks.GetFavoriteBook(userId, bookId);

            if (favorite == null)
                return false;

            _unitOfWork.FavoriteBooks.Delete(favorite);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
