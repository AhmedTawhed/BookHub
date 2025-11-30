using BookHub.Core.DTOs.FavoriteBookDto;
using BookHub.Core.Entities;
using BookHub.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task AddFavoriteBook(string userId, FavoriteBookResponseDto dto)
        {
            var exists = await _unitOfWork.FavoriteBooks.GetFavoriteBook(userId, dto.BookId);

            if (exists != null)
                throw new Exception("Book already in favorites.");

            var favorite = new FavoriteBook
            {
                UserId = userId,
                BookId = dto.BookId
            };

            await _unitOfWork.FavoriteBooks.Add(favorite);
            await _unitOfWork.CompleteAsync();
        }

        public async Task RemoveFavoriteBook(string userId, int bookId)
        {
            var favorite = await _unitOfWork.FavoriteBooks.GetFavoriteBook(userId, bookId);

            if (favorite == null)
                throw new Exception("Favorite book not found.");

            _unitOfWork.FavoriteBooks.Delete(favorite);
            await _unitOfWork.CompleteAsync();
        }


    }
}
