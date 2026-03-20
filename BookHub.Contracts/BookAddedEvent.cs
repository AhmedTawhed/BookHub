namespace BookHub.Contracts;

public class BookAddedEvent
{
    public int BookId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}
