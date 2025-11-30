using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Helpers.CustomRequests;

namespace BookHub.Core.Interfaces.IService
{
    public interface IBookService
    {
        Task<BookResponseDto> AddBook(BookRequestDto dto);
        Task<bool> DeleteBook(int id);
        Task<IEnumerable<BookResponseDto>> GetAllBooks();
        Task<BookResponseDto?> GetBookById(int id);
        Task<BookResponseDto> UpdateBook(int id, BookRequestDto dto);
        Task<IEnumerable<BookResponseDto>> GetPaged(GridRequest request);
    }
}