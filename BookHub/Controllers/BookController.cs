using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Interfaces.IService;
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
        public async Task<IActionResult> Addbook([FromBody] BookRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdBook = await _bookService.AddBook(dto);

            return CreatedAtAction(nameof(GetBookById), new { id = createdBook.Id }, createdBook);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] BookRequestDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            
            var updatedBook = await _bookService.UpdateBook(id, dto);

            if (updatedBook == null)
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

        [HttpPost]
        public async Task<IActionResult> GetPagedBooks([FromBody] GridRequest request)
        {
            var book = await _bookService.GetPaged(request);

            if (book == null)
                return NotFound();

            return Ok(book);
        }
    }
}