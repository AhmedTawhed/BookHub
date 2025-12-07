using BookHub.Core.DTOs.ReviewDtos;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces.Service;
using BookHub.Core.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookHub.API.Controllers
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

        private string GetUserId()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                throw new UnauthorizedException("User is not authenticated.");
            return userId;
        }

        [Authorize(Roles = "User")]
        [HttpGet("user")]
        public async Task<IActionResult> GetUserReviews()
        {
            var userId = GetUserId();
            var reviews = await _reviewService.GetReviewsByUser(userId);

            return Ok(ApiResponse<IEnumerable<ReviewResponseDto>>.Ok(reviews, "Reviews retrieved successfully"));
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReviewById(int id)
        {
            var review = await _reviewService.GetReviewById(id);
            return Ok(ApiResponse<ReviewResponseDto>.Ok(review, "Review retrieved successfully"));
        }

        [Authorize]
        [HttpGet("book/{bookId}")]
        public async Task<IActionResult> GetReviewsByBook(int bookId)
        {
            var reviews = await _reviewService.GetReviewsByBook(bookId);
            return Ok(ApiResponse<IEnumerable<ReviewResponseDto>>.Ok(reviews, "Reviews retrieved successfully"));
        }

        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> AddReview([FromBody] ReviewRequestDto dto)
        {
            var userId = GetUserId(); 
            dto.UserId = userId;

            var createdReview = await _reviewService.AddReview(dto);

            return CreatedAtAction(nameof(GetReviewById), new { id = createdReview.Id }, ApiResponse<ReviewResponseDto>.Ok(createdReview, "Review added successfully"));

        }

        [Authorize(Roles = "User")]
        [HttpPut("{reviewId}")]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewRequestDto dto)
        {
            var userId = GetUserId();
            var updatedReview = await _reviewService.UpdateReview(userId, reviewId, dto);

            return Ok(ApiResponse<ReviewResponseDto>.Ok(updatedReview, "Review updated successfully"));
        }
    }
}
