namespace BookHub.Core.Helpers.CustomRequests;

public class GridRequest
{
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public string? SortColumn { get; set; }
    public string? SortDirection { get; set; } = "asc";
    public string? SearchText { get; set; }
}