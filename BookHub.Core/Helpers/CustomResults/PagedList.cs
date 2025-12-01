namespace BookHub.Core.Helpers.CustomResults;

public class PagedList<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int NumberOfPages { get; set; }
    public int TotalCount { get; set; }
}