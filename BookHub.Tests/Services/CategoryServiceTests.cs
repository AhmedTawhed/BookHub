using BookHub.Core.DTOs.CategoryDtos;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using System.Linq.Expressions;

namespace BookHub.Tests.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<ICategoryRepository> _mockCategoryRepo;
        private readonly IMemoryCache _cache;
        private readonly CategoryService _service;

        public CategoryServiceTests()
        {
            _mockCategoryRepo = new Mock<ICategoryRepository>();
            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.Setup(u => u.Categories).Returns(_mockCategoryRepo.Object);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _service = new CategoryService(_mockUoW.Object, _cache);
        }

        [Fact]
        public async Task GetAllCategories_ReturnsAllDtos()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" },
                new Category { Id = 2, Name = "Science" }
            };
            _mockCategoryRepo.Setup(r => r.GetAll()).ReturnsAsync(categories);

            var result = await _service.GetAllCategories();

            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Fiction");
            result.Last().Name.Should().Be("Science");
            _mockCategoryRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetAllCategories_SecondCall_ReturnsCachedResult()
        {
            var categories = new List<Category>
            {
                new Category { Id = 1, Name = "Fiction" }
            };
            _mockCategoryRepo.Setup(r => r.GetAll()).ReturnsAsync(categories);

            await _service.GetAllCategories();
            await _service.GetAllCategories();

            _mockCategoryRepo.Verify(r => r.GetAll(), Times.Once);
        }

        [Fact]
        public async Task GetCategoryById_Exists_ReturnsDto()
        {
            var category = new Category { Id = 1, Name = "Fantasy" };
            _mockCategoryRepo.Setup(r => r.GetById(1)).ReturnsAsync(category);

            var result = await _service.GetCategoryById(1);

            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Fantasy");
        }

        [Fact]
        public async Task GetCategoryById_NotFound_ThrowsNotFoundException()
        {
            _mockCategoryRepo.Setup(r => r.GetById(99)).ReturnsAsync((Category?)null);

            Func<Task> act = async () => await _service.GetCategoryById(99);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category not found");
        }

        [Fact]
        public async Task AddCategory_ValidRequest_AddsAndReturnsDto()
        {
            var dto = new CategoryRequestDto { Name = "Thriller" };
            _mockCategoryRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new List<Category>());

            var result = await _service.AddCategory(dto);

            result.Should().NotBeNull();
            result.Name.Should().Be("Thriller");
            _mockCategoryRepo.Verify(r => r.Add(It.Is<Category>(c => c.Name == "Thriller")), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddCategory_DuplicateName_ThrowsBadRequestException()
        {
            var dto = new CategoryRequestDto { Name = "Fiction" };
            _mockCategoryRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Category, bool>>>()))
                .ReturnsAsync(new List<Category> { new Category { Id = 1, Name = "Fiction" } });

            Func<Task> act = async () => await _service.AddCategory(dto);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Category with the same name already exists");
            _mockCategoryRepo.Verify(r => r.Add(It.IsAny<Category>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateCategory_Exists_UpdatesAndReturnsDto()
        {
            var existing = new Category { Id = 1, Name = "Old Name" };
            var dto = new CategoryRequestDto { Name = "New Name" };
            _mockCategoryRepo.Setup(r => r.GetById(1)).ReturnsAsync(existing);

            var result = await _service.UpdateCategory(1, dto);

            result.Name.Should().Be("New Name");
            existing.Name.Should().Be("New Name");
            _mockCategoryRepo.Verify(r => r.Update(existing), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateCategory_NotFound_ThrowsNotFoundException()
        {
            _mockCategoryRepo.Setup(r => r.GetById(99)).ReturnsAsync((Category?)null);

            Func<Task> act = async () => await _service.UpdateCategory(99, new CategoryRequestDto { Name = "X" });

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category not found");
            _mockCategoryRepo.Verify(r => r.Update(It.IsAny<Category>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteCategory_Exists_DeletesAndSaves()
        {
            var category = new Category { Id = 1, Name = "Romance" };
            _mockCategoryRepo.Setup(r => r.GetById(1)).ReturnsAsync(category);

            await _service.DeleteCategory(1);

            _mockCategoryRepo.Verify(r => r.Delete(category), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteCategory_NotFound_ThrowsNotFoundException()
        {
            _mockCategoryRepo.Setup(r => r.GetById(99)).ReturnsAsync((Category?)null);

            Func<Task> act = async () => await _service.DeleteCategory(99);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category not found");
            _mockCategoryRepo.Verify(r => r.Delete(It.IsAny<Category>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Theory]
        [InlineData(0, 10, 8, 1)]
        [InlineData(0, 5, 8, 2)]
        [InlineData(5, 5, 8, 2)]
        public async Task GetPagedCategories_VariousRequests_ReturnsCorrectMetadata(
            int skip, int take, int totalCount, int expectedPages)
        {
            var items = Enumerable.Range(1, Math.Min(take, totalCount - skip))
                .Select(i => new Category { Id = i, Name = $"Category {i}" })
                .ToList();

            var pagedList = new PagedList<Category>
            {
                Items = items,
                TotalCount = totalCount,
                NumberOfPages = expectedPages
            };
            var request = new GridRequest { Skip = skip, Take = take };
            _mockCategoryRepo.Setup(r => r.GetPage(It.IsAny<GridRequest>(), null))
                .ReturnsAsync(pagedList);

            var result = await _service.GetPagedCategories(request);

            result.TotalCount.Should().Be(totalCount);
            result.NumberOfPages.Should().Be(expectedPages);
            result.Items.Should().BeEquivalentTo(items, options => options.ExcludingMissingMembers());
        }
    }
}
