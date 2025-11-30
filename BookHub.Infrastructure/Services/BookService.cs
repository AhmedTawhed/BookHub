using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Entities;
using BookHub.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookDto?> GetBookById(int id)
        {
            var book = await _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                return null;
            }

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId
            };
        }

        public async Task<IEnumerable<BookDto>> GetAllBooks()
        {
            var books = await _unitOfWork.Books.GetAll();

            return books.Select(book => new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId
            });
        }
        public async Task<BookDto> AddBook(BookResponseDto dto)
        {
            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            };

            await _unitOfWork.Books.Add(book);
            await _unitOfWork.CompleteAsync();

            return new BookDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId
            };
        }
        public async Task<bool> UpdateBook(int id, BookResponseDto dto)
        {
            var book = await _unitOfWork.Books.GetById(id);

            if (book == null)
            {
                return false;
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Description = dto.Description;
            book.CategoryId = dto.CategoryId;

            _unitOfWork.Books.Update(book);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteBook(int id)
        {
            var book = await _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                return false;
            }

            _unitOfWork.Books.Delete(book);
            await _unitOfWork.CompleteAsync();

            return true;
        }
    }
}
