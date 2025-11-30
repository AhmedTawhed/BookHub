using BookHub.Core.DTOs.CategoryDtos;

namespace BookHub.Infrastructure.Services
{
    public interface ICategoryService
    {
        Task<CategoryDto> AddCategory(CategoryResponseDto dto);
        Task<bool> DeleteCategory(int id);
        Task<IEnumerable<CategoryDto>> GetAllCategories();
        Task<CategoryDto?> GetCategoryById(int id);
        Task<bool> UpdateCategory(int id, CategoryResponseDto dto);
    }
}