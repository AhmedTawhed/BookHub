using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.DTOs.CategoryDtos;
using BookHub.Core.Entities;
using BookHub.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookHub.Infrastructure.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllCategories()
        {
            var categories = await _unitOfWork.Categories.GetAll();

            return categories.Select(c => new CategoryDto
            {
                Id = c.Id,
                Name = c.Name
            });
        }

        public async Task<CategoryDto?> GetCategoryById(int id)
        {
            var category = await _unitOfWork.Categories.GetById(id);

            if (category == null)
                return null;

            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<CategoryDto> AddCategory(CategoryResponseDto dto)
        {
            var category = new Category
            {
                Name = dto.Name
            };

            await _unitOfWork.Categories.Add(category);
            await _unitOfWork.CompleteAsync();
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public async Task<bool> UpdateCategory(int id, CategoryResponseDto dto)
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
    }
}
