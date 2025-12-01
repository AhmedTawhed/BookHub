using System.ComponentModel.DataAnnotations;

namespace BookHub.Core.DTOs.BookDtos
{
    public class BookRequestDto
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Author is required")]
        [StringLength(100, ErrorMessage = "Author can't be longer than 100 characters")]
        public string Author { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "Description can't be longer than 1000 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "CategoryId is required")]
        public int CategoryId { get; set; }
    }
}