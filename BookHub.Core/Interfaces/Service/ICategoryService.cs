using BookHub.Core.DTOs.CategoryDtos;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;

namespace BookHub.Core.Interfaces.Service
{
    public interface ICategoryService
    {
        Task<CategoryResponseDto> AddCategory(CategoryRequestDto dto);
        Task<bool> DeleteCategory(int id);
        Task<IEnumerable<CategoryResponseDto>> GetAllCategories();
        Task<CategoryResponseDto?> GetCategoryById(int id);
        Task<bool> UpdateCategory(int id, CategoryRequestDto dto);
        Task<PagedList<CategoryResponseDto>> GetPagedCategories(GridRequest request);
    }
}