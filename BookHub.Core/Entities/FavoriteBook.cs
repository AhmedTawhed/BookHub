namespace BookHub.Core.Entities
{
    public class FavoriteBook
    {
        public int BookId { get; set; }
        public Book Book { get; set; } = default!;
        public string UserId { get; set; } = default!;
        public ApplicationUser User { get; set; } = default!;
    }
}
