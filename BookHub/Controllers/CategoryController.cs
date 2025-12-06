using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.DTOs.CategoryDtos;
using BookHub.Core.Entities;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces.Service;
using BookHub.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [AllowAnonymous]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryService.GetAllCategories();
            return Ok(ApiResponse<IEnumerable<CategoryResponseDto>>.Ok(categories, "Categories retrieved successfully"));
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var category = await _categoryService.GetCategoryById(id);
            return Ok(ApiResponse<CategoryResponseDto>.Ok(category, "Category retrieved successfully"));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCategory([FromBody] CategoryRequestDto dto)
        {
            var createdCategory = await _categoryService.AddCategory(dto);
            return CreatedAtAction(nameof(GetCategoryById), new { id = createdCategory.Id }, ApiResponse<CategoryResponseDto>.Ok(createdCategory, "Category created successfully"));

        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryRequestDto dto)
        {
            var updatedCategory = await _categoryService.UpdateCategory(id, dto);
            return Ok(ApiResponse<CategoryResponseDto>.Ok(updatedCategory, "Category updated successfully"));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            await _categoryService.DeleteCategory(id);
            return Ok(ApiResponse<string>.Ok(null, "Category deleted successfully"));
        }

        [AllowAnonymous]
        [HttpGet("paged")]
        public async Task<IActionResult> GetPagedCategories([FromQuery] GridRequest request)
        {
            var result = await _categoryService.GetPagedCategories(request);
            return Ok(ApiResponse<PagedList<CategoryResponseDto>>.Ok(result, "Paged categories retrieved successfully"));
        }
    }
}
