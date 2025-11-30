using BookHub.Core.DTOs.BookDtos;
using BookHub.Infrastructure.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            var books = await _bookService.GetAllBooks();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            var book = await _bookService.GetBookById(id);

            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Addbook([FromBody] BookResponseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdBook = await _bookService.AddBook(dto);

            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookResponseDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var updated = await _bookService.UpdateBook(id, dto);

            if (!updated)
                return NotFound();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var deleted = await _bookService.DeleteBook(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
