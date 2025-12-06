using BookHub.Core.DTOs.FavoriteBookDtos;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;

namespace BookHub.Infrastructure.Services
{
    public class FavoriteBookService : IFavoriteBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public FavoriteBookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        private FavoriteBookDto MapToDto(FavoriteBook favoriteBook)
        {
            return new FavoriteBookDto
            {
                BookId = favoriteBook.BookId,
                BookTitle = favoriteBook.Book.Title,
                UserId = favoriteBook.UserId
            };
        }

        public async Task<IEnumerable<FavoriteBookDto>> GetUserFavoriteBooks(string userId)
        {
            var favoriteBooks = await _unitOfWork.FavoriteBooks.GetFavoriteBooksByUserId(userId);
            return favoriteBooks.Select(MapToDto);     
        }

        public async Task<FavoriteBookDto> AddFavoriteBook(string userId, int bookId)
        {
            var exists = await _unitOfWork.FavoriteBooks.GetFavoriteBook(userId, bookId);

            if (exists != null)
                throw new BadRequestException("Book already in favorites.");

            var book = await _unitOfWork.Books.GetById(bookId);
            if (book == null)
                throw new NotFoundException("Book does not exist.");

            var favorite = new FavoriteBook
            {
                UserId = userId,
                BookId = bookId
            };

            await _unitOfWork.FavoriteBooks.Add(favorite);
            await _unitOfWork.CompleteAsync();

            return MapToDto(favorite);
        }

        public async Task RemoveFavoriteBook(string userId, int bookId)
        {
            var favorite = await _unitOfWork.FavoriteBooks.GetFavoriteBook(userId, bookId);

            if (favorite == null)
                throw new NotFoundException("Favorite book not found.");

            _unitOfWork.FavoriteBooks.Delete(favorite);
            await _unitOfWork.CompleteAsync();
        }
    }
}
