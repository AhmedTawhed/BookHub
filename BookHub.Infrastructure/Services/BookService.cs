using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;
using Microsoft.Extensions.Caching.Memory;
using System.Linq.Dynamic.Core;

namespace BookHub.Infrastructure.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private const string CacheKey = "books_all";

        public BookService(IUnitOfWork unitOfWork, IMemoryCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        private BookResponseDto MapToDto(Book book)
        {
            return new BookResponseDto
            {
                Id = book.Id,
                Title = book.Title,
                Author = book.Author,
                Description = book.Description,
                CategoryId = book.CategoryId,
                CategoryName = book.Category?.Name ?? string.Empty,
                ReviewCount = book.Reviews.Count,
                AverageRating = book.Reviews.Count > 0
                    ? Math.Round(book.Reviews.Average(r => r.Rating), 1)
                    : 0
            };
        }

        public async Task<BookResponseDto?> GetBookById(int id)
        {
            var book = await _unitOfWork.Books.GetByIdWithReviews(id);
            if (book == null)
                throw new NotFoundException("Book not found");

            return MapToDto(book);
        }

        public async Task<IEnumerable<BookResponseDto>> GetAllBooks()
        {
            if (_cache.TryGetValue(CacheKey, out IEnumerable<BookResponseDto>? cached))
                return cached!;

            var books = await _unitOfWork.Books.GetBooksWithReviews();
            var result = books.Select(MapToDto).ToList();

            _cache.Set(CacheKey, result, TimeSpan.FromMinutes(10));

            return result;
        }
        public async Task<BookResponseDto> AddBook(BookRequestDto dto)
        {
            var category = await _unitOfWork.Categories.GetById(dto.CategoryId);
            if (category == null) 
                throw new NotFoundException("Category not found");

            var existingBook = (await _unitOfWork.Books
                .Find(b => b.Title.ToLower() == dto.Title.ToLower())).FirstOrDefault();
            if (existingBook != null)
                throw new BadRequestException("A book with the same title already exists.");

            var book = new Book
            {
                Title = dto.Title,
                Author = dto.Author,
                Description = dto.Description,
                CategoryId = dto.CategoryId
            };

            await _unitOfWork.Books.Add(book);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(CacheKey);

            return MapToDto(book);
        }
        public async Task<BookResponseDto> UpdateBook(int id, BookRequestDto dto)
        {
            var book = await _unitOfWork.Books.GetById(id);

            if (book == null)
                throw new NotFoundException("Book not found");

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.Description = dto.Description;
            book.CategoryId = dto.CategoryId;

            _unitOfWork.Books.Update(book);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(CacheKey);

            return MapToDto(book);
        }

        public async Task DeleteBook(int id)
        {
            var book = await _unitOfWork.Books.GetById(id);
            if (book == null)
                throw new NotFoundException("Book not found");

            _unitOfWork.Books.Delete(book);
            await _unitOfWork.CompleteAsync();
            _cache.Remove(CacheKey);
        }

        public async Task<PagedList<BookResponseDto>> GetPagedBooks(GridRequest request)
        {
            var pagedBooks = await _unitOfWork.Books.GetPagedBooksWithReviews(request);

            var bookDtos = pagedBooks.Items.Select(MapToDto);

            return new PagedList<BookResponseDto>
            {
                Items = bookDtos,
                TotalCount = pagedBooks.TotalCount,
                NumberOfPages = pagedBooks.NumberOfPages
            };
        }
    }
}