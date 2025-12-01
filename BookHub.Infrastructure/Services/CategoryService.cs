using BookHub.Core.DTOs.CategoryDtos;
using BookHub.Core.Entities;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Service;

namespace BookHub.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAll();

            return categories.Select(c => new CategoryResponseDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryResponseDto?> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
                return null;

            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryResponseDto> AddCategory(CategoryRequestDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            await _unitOfWork.Categories.Add(category);
            await _unitOfWork.CompleteAsync();
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<bool> UpdateCategory(int id, CategoryRequestDto dto)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
                return false;

            category.Name = dto.Name;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
                return false;

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CompleteAsync();

            return true;
        }

        public async Task<PagedList<CategoryResponseDto>> GetPagedCategories(GridRequest request)
        {
            var pagedCategories = await _unitOfWork.Categories.GetPage(request);

            var categoryDtos = pagedCategories.Items.Select(c => new CategoryResponseDto
            { 
                Id = c.Id,
                Name = c.Name
            });
            return new PagedList<CategoryResponseDto>
            {
                Items = categoryDtos,
                NumberOfPages = pagedCategories.NumberOfPages,
                TotalCount = pagedCategories.TotalCount
            };
        }
    }
}
