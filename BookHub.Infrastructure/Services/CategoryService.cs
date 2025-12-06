using BookHub.Core.DTOs.CategoryDtos;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
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

        private CategoryResponseDto MapToDto(Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }
        public async Task<IEnumerable<CategoryResponseDto>> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAll();

            return categories.Select(MapToDto);
        }

        public async Task<CategoryResponseDto?> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
                throw new NotFoundException("Category not found");

            return MapToDto(category);
        }

        public async Task<CategoryResponseDto> AddCategory(CategoryRequestDto dto)
        {
            var existingCategory = (await _unitOfWork.Categories
                .Find(c => c.Name.ToLower() == dto.Name.ToLower())).FirstOrDefault();
            if (existingCategory != null)
                throw new BadRequestException("Category with the same name already exists");

            var category = new Category
            {
                Name = dto.Name
            };

            await _unitOfWork.Categories.Add(category);
            await _unitOfWork.CompleteAsync();

            return MapToDto(category);
        }

        public async Task<CategoryResponseDto> UpdateCategory(int id, CategoryRequestDto dto)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
                throw new NotFoundException("Category not found");

            category.Name = dto.Name;

            _unitOfWork.Categories.Update(category);
            await _unitOfWork.CompleteAsync();

            return MapToDto(category);
        }

        public async Task DeleteCategory(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);
            if (category == null)
                throw new NotFoundException("Category not found");

            _unitOfWork.Categories.Delete(category);
            await _unitOfWork.CompleteAsync();
        }

        public async Task<PagedList<CategoryResponseDto>> GetPagedCategories(GridRequest request)
        {
            var pagedCategories = await _unitOfWork.Categories.GetPage(request);

            var categoryDtos = pagedCategories.Items.Select(MapToDto);

            return new PagedList<CategoryResponseDto>
            {
                Items = categoryDtos,
                NumberOfPages = pagedCategories.NumberOfPages,
                TotalCount = pagedCategories.TotalCount
            };
        }
    }
}
