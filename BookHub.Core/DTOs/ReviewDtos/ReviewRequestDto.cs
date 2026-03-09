namespace BookHub.Core.DTOs.ReviewDtos
{
    public class ReviewRequestDto
    {
        public int BookId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
