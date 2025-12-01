using System.ComponentModel.DataAnnotations;

namespace BookHub.Core.DTOs.ReviewDtos
{
    public class ReviewRequestDto
    {
        [Required(ErrorMessage = "BookId is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment can't exceed 500 characters")]
        public string? Comment { get; set; }
    }
}
