using System.ComponentModel.DataAnnotations;

namespace BookHub.Core.DTOs.CategoryDtos
{
    public class CategoryRequestDto
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name can't be longer than 50 characters")]
        public string Name { get; set; } = string.Empty;
    }
}
