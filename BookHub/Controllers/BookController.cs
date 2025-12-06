using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces.Service;
using BookHub.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace BookHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(ApiResponse<IEnumerable<BookResponseDto>>.Ok(books, "Books retrieved successfully"));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);
            return Ok(ApiResponse<BookResponseDto>.Ok(book, "Book retrieved successfully"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Addbook([FromBody] BookRequestDto dto)
        {
            var createdBook = await _bookService.AddBook(dto);
            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id },
                ApiResponse<BookResponseDto>.Ok(createdBook, "Book created successfully"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequestDto dto)
        {
            var updatedBook = await _bookService.UpdateBook(id, dto);
            return Ok(ApiResponse<BookResponseDto>.Ok(updatedBook, "Book updated successfully"));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBook(id);
            return Ok(ApiResponse<string>.Ok(null, "Book deleted successfully"));
        }

        [AllowAnonymous]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedBooks([FromQuery] GridRequest request)
        {
            var result = await _bookService.GetPagedBooks(request);
            return Ok(ApiResponse<PagedList<BookResponseDto>>.Ok(result, "Paged books retrieved successfully"));
        }
    }
}