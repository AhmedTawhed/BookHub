using BookHub.Core.Interfaces.Service;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteBookController : ControllerBase
    {
        private readonly IFavoriteBookService _favoriteBookService;

        public FavoriteBookController(IFavoriteBookService favoriteBookService)
        {
            _favoriteBookService = favoriteBookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUserFavoriteBooks()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var favoriteBooks = await _favoriteBookService.GetUserFavoriteBooks(userId);

            return Ok(favoriteBooks);
        }

        [HttpPost("{bookId}")]
        public async Task<IActionResult> AddFavoriteBook(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var createdFavorite =  await _favoriteBookService.AddFavoriteBook(userId, bookId);

            return CreatedAtAction(nameof(GetUserFavoriteBooks), new { }, createdFavorite);
        }

        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFavoriteBook(int bookId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var result = await _favoriteBookService.RemoveFavoriteBook(userId, bookId);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
