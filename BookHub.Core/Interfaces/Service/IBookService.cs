using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;

namespace BookHub.Core.Interfaces.Service
{
    public interface IBookService
    {
        Task<BookResponseDto> AddBook(BookRequestDto dto);
        Task<bool> DeleteBook(int id);
        Task<IEnumerable<BookResponseDto>> GetAllBooks();
        Task<BookResponseDto?> GetBookById(int id);
        Task<BookResponseDto> UpdateBook(int id, BookRequestDto dto);
        Task<PagedList<BookResponseDto>> GetPagedBooks(GridRequest request);
    }
}