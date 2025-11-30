using BookHub.Core.DTOs.BookDtos;

namespace BookHub.Infrastructure.Services
{
    public interface IBookService
    {
        Task<BookDto> AddBook(BookResponseDto dto);
        Task<bool> DeleteBook(int id);
        Task<IEnumerable<BookDto>> GetAllBooks();
        Task<BookDto?> GetBookById(int id);
        Task<bool> UpdateBook(int id, BookResponseDto dto);
    }
}