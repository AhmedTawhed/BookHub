using BookHub.Core.DTOs.FavoriteBookDtos;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces.Service;
using BookHub.Responses;
using Microsoft.AspNetCore.Authorization;
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

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UnauthorizedException("User is not authenticated.");
            return userId;
        }

        [Authorize(Roles = "User")]
        [HttpGet]
        public async Task<IActionResult> GetUserFavoriteBooks()
        {
            var userId = GetUserId();
            var favoriteBooks = await _favoriteBookService.GetUserFavoriteBooks(userId);

            return Ok(ApiResponse<IEnumerable<FavoriteBookDto>>.Ok(favoriteBooks, "Favorite books retrieved successfully"));

        }

        [Authorize(Roles = "User")]
        [HttpPost("{bookId}")]
        public async Task<IActionResult> AddFavoriteBook(int bookId)
        {
            var userId = GetUserId();
            var createdFavorite =  await _favoriteBookService.AddFavoriteBook(userId, bookId);

            return Created(string.Empty, createdFavorite);
        }

        [Authorize(Roles = "User")]
        [HttpDelete("{bookId}")]
        public async Task<IActionResult> RemoveFavoriteBook(int bookId)
        {
            var userId = GetUserId();
            await _favoriteBookService.RemoveFavoriteBook(userId, bookId);

            return Ok(ApiResponse<string>.Ok(null, "Book removed from favorites successfully"));
        }
    }
}
