using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Entities;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;
using System.Linq.Dynamic.Core;

namespace BookHub.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;

        public BookService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookResponseDto?> GetBookById(int id)
        {
            var book = await _unitOfWork.Books.GetById(id);
            if (book == null)
            {
                return null;
            }

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId
            };
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooks()
        {
            var books = await _unitOfWork.Books.GetAll();

            return books.Select(book => new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId
            });
        }
        public async Task<BookResponseDto> AddBook(BookRequestDto dto)
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

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId
            };
        }
        public async Task<BookResponseDto> UpdateBook(int id, BookRequestDto dto)
        {
            var book = await _unitOfWork.Books.GetById(id);

            if (book == null)
                throw new Exception("Book not found.");

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Description = dto.Description;
            book.CategoryId = dto.CategoryId;

            _unitOfWork.Books.Update(book);
            await _unitOfWork.CompleteAsync();

            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId
            };
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

        public async Task<PagedList<BookResponseDto>> GetPagedBooks(GridRequest request)
        {
            var pagedBooks = await _unitOfWork.Books.GetPage(request);

            var bookDtos = pagedBooks.Items.Select(b => new BookResponseDto
            {
                Id = b.Id,
                Title = b.Title,
                Author = b.Author,
                Description = b.Description,
                CategoryId = b.CategoryId
            });

            return new PagedList<BookResponseDto>
            {
                Items = bookDtos,
                TotalCount = pagedBooks.TotalCount,
                NumberOfPages = pagedBooks.NumberOfPages
            };
        }
    }
}