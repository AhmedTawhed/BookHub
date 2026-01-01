namespace BookHub.Core.DTOs.ReviewDtos
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public int BookId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
