namespace BookHub.Contracts;

public class ReviewAddedEvent
{
    public int BookId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public int Rating { get; set; }
}
