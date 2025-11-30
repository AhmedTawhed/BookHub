using BookHub.Core.DTOs.ReviewDto;
using BookHub.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetUserReviews()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            var reviews = await _reviewService.GetReviewsByUser(userId);
            return Ok(reviews);
        }

        [HttpGet("book/{bookId}")]
        [AllowAnonymous] 
        public async Task<IActionResult> GetReviewsByBook(int bookId)
        {
            var reviews = await _reviewService.GetReviewsByBook(bookId);
            return Ok(reviews);
        }

        [HttpPost("book/{bookId}")]
        public async Task<IActionResult> AddReview(int bookId, [FromBody] ReviewResponseDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdReview = await _reviewService.AddReview(userId, bookId, dto);

            return CreatedAtAction(nameof(GetReviewsByBook), new { bookId }, createdReview);
        }

        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewResponseDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updatedReview = await _reviewService.UpdateReview(userId, reviewId, dto);

            return Ok(updatedReview);
        }
    }
}
