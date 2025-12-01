using System.ComponentModel.DataAnnotations;

namespace BookHub.Core.DTOs.FavoriteBookDtos
{
    public class FavoriteBookDto
    {
        [Required(ErrorMessage = "BookId is required")]
        public int BookId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        public string BookTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "UserId is required")]
        public string UserId { get; set; } = string.Empty;
    }

}
