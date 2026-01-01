using BookHub.Core.DTOs.BookDtos;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Helpers.CustomRequests;
using BookHub.Core.Helpers.CustomResults;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Services;
using FluentAssertions;
using Moq;
using System.Linq.Expressions;

namespace BookHub.Tests.Services
{
    public class BookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<IBookRepository> _mockBooksRepo;
        private readonly Mock<ICategoryRepository> _mockCategoriesRepo;
        private readonly BookService _service;

        public BookServiceTests()
        {
            _mockBooksRepo = new Mock<IBookRepository>();
            _mockCategoriesRepo = new Mock<ICategoryRepository>();
            _mockUoW = new Mock<IUnitOfWork>();

            _mockUoW.Setup(u => u.Books).Returns(_mockBooksRepo.Object);
            _mockUoW.Setup(u => u.Categories).Returns(_mockCategoriesRepo.Object);

            _service = new BookService(_mockUoW.Object);
        }

        [Fact]
        public async Task GetBookById_BookExists_ReturnsBookDto()
        {
            var bookId = 1;
            var book = new Book
            {
                Id = bookId,
                Title = "Test Book",
                Author = "Test Author",
                Description = "Test Description",
                CategoryId = 1
            };
            _mockBooksRepo.Setup(r => r.GetById(bookId)).ReturnsAsync(book);

            var result = await _service.GetBookById(bookId);

            result.Should().NotBeNull();
            result.Id.Should().Be(bookId);
            result.Title.Should().Be("Test Book");
            result.Author.Should().Be("Test Author");
            result.Description.Should().Be("Test Description");
            result.CategoryId.Should().Be(1);
        }
        [Fact]

        public async Task GetBookById_BookDoesNotExist_ThrowsNotFoundException()
        {
            var bookId = 1;
            _mockBooksRepo.Setup(r => r.GetById(bookId)).ReturnsAsync((Book?)null);
         
            Func<Task> act = async () => await _service.GetBookById(bookId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Book not found");
        }

        [Fact]
        public async Task GetAllBooks_ReturnsListOfBookDtos()
        {
            var books = new List<Book>
            {
                new Book { Id = 1, Title = "Book 1", Author = "Author 1", Description = "Desc 1", CategoryId = 1 },
                new Book { Id = 2, Title = "Book 2", Author = "Author 2", Description = "Desc 2", CategoryId = 2 }
            };
            _mockBooksRepo.Setup(r => r.GetAll()).ReturnsAsync(books);

            var result = await _service.GetAllBooks();

            result.Should().HaveCount(2);
            result.First().Title.Should().Be("Book 1");
            result.Last().Title.Should().Be("Book 2");
            _mockBooksRepo.Verify(b => b.GetAll(), Times.Once);
        }

        [Theory]
        [InlineData("New Book 1", "New Author 1", "New Description 1", 1)]
        [InlineData("New Book 2", "New Author 2", "New Description 2", 2)]
        public async Task AddBook_ValidRequest_AddsBookAndReturnsDto(
            string title, string author, string description, int categoryId)
        {
            var bookRequest = new BookRequestDto
            {
                Title = title,
                Author = author,
                Description = description,
                CategoryId = categoryId
            };
            var category = new Category { Id = categoryId, Name = "Category" + categoryId };
            _mockCategoriesRepo.Setup(r => r.GetById(bookRequest.CategoryId)).ReturnsAsync(category);
            _mockBooksRepo.Setup(r => r.Find(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(new List<Book>());

            var result = await _service.AddBook(bookRequest);

            result.Should().NotBeNull();
            result.Title.Should().Be(title);
            result.Author.Should().Be(author);
            result.Description.Should().Be(description);
            result.CategoryId.Should().Be(categoryId);
            _mockBooksRepo.Verify(r => r.Add(It.Is<Book>(b => b.Title == title)), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddBook_DuplicateTitle_ThrowsBadRequest()
        {
            var dto = new BookRequestDto { Title = "Book", CategoryId = 1 };

            _mockCategoriesRepo.Setup(c => c.GetById(dto.CategoryId))
                .ReturnsAsync(new Category { Id = 1 });

            _mockBooksRepo.Setup(b => b.Find(It.IsAny<Expression<Func<Book, bool>>>()))
                .ReturnsAsync(new List<Book> { new Book { Title = "Book" } });

            Func<Task> act = async () => await _service.AddBook(dto);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("A book with the same title already exists.");
            _mockBooksRepo.Verify(b => b.Add(It.IsAny<Book>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task AddBook_CategoryDoesNotExist_ThrowsNotFoundException()
        {
            var dto = new BookRequestDto
            {
                Title = "New Book",
                Author = "New Author",
                Description = "New Description",
                CategoryId = 99
            };

            _mockCategoriesRepo.Setup(c => c.GetById(dto.CategoryId))
                .ReturnsAsync((Category?)null);

            Func<Task> act = async () => await _service.AddBook(dto);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Category not found");
            _mockBooksRepo.Verify(b => b.Add(It.IsAny<Book>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateBook_BookExists_UpdatesBookAndReturnsDto()
        {
            var bookId = 1;
            var existingBook = new Book
            {
                Id = bookId,
                Title = "Old Title",
                Author = "Old Author",
                Description = "Old Desc",
                CategoryId = 1
            };

            var dto = new BookRequestDto
            {
                Title = "Updated Title",
                Author = "Updated Author",
                Description = "Updated Desc",
                CategoryId = 2
            };

            _mockBooksRepo.Setup(b => b.GetById(bookId))
                .ReturnsAsync(existingBook);

            var result = await _service.UpdateBook(bookId, dto);

            result.Should().NotBeNull();
            result.Title.Should().Be(dto.Title);
            result.Author.Should().Be(dto.Author);
            result.Description.Should().Be(dto.Description);
            result.CategoryId.Should().Be(dto.CategoryId);
            existingBook.Title.Should().Be(dto.Title);
            existingBook.Author.Should().Be(dto.Author);
            existingBook.Description.Should().Be(dto.Description);
            existingBook.CategoryId.Should().Be(dto.CategoryId);
            _mockBooksRepo.Verify(b => b.Update(existingBook), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateBook_BookDoesNotExist_ThrowsNotFoundException()
        {
            var bookId = 1;
            var dto = new BookRequestDto
            {
                Title = "Updated Title",
                Author = "Updated Author",
                Description = "Updated Desc",
                CategoryId = 2
            };

            _mockBooksRepo.Setup(b => b.GetById(bookId))
                .ReturnsAsync((Book?)null);

            Func<Task> act = async () => await _service.UpdateBook(bookId, dto);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Book not found");
            _mockBooksRepo.Verify(b => b.Update(It.IsAny<Book>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteBook_BookExists_DeletesBookAndSaves()
        {
            var bookId = 1;

            var existingBook = new Book
            {
                Id = bookId,
                Title = "Book to Delete",
            };

            _mockBooksRepo.Setup(b => b.GetById(bookId))
                .ReturnsAsync(existingBook);

            await _service.DeleteBook(bookId);

            _mockBooksRepo.Verify(b => b.Delete(existingBook), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteBook_BookDoesNotExist_ThrowsNotFoundException()
        {
            var bookId = 1;
            _mockBooksRepo.Setup(b => b.GetById(bookId))
                .ReturnsAsync((Book?)null);

            Func<Task> act = async () => await _service.DeleteBook(bookId);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Book not found");
            _mockBooksRepo.Verify(b => b.Delete(It.IsAny<Book>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Theory]
        [InlineData(0, 10, 12, 2)]
        [InlineData(10, 5, 12, 3)]
        [InlineData(0, 5, 3, 1)]
        public async Task GetPagedBooks_VariousRequests_ReturnsCorrectDataAndMetadata(
            int skip, int take, int totalCount, int expectedPages)
        {
            var items = Enumerable.Range(skip + 1, Math.Min(take, totalCount - skip))
                                  .Select(i => new Book { Id = i, Title = $"Book {i}", Author = $"Author {i}" })
                                  .ToList();

            var pagedList = new PagedList<Book>
            {
                Items = items,
                TotalCount = totalCount,
                NumberOfPages = expectedPages
            };
            var gridRequest = new GridRequest { Skip = skip, Take = take };
            _mockBooksRepo.Setup(b => b.GetPage(It.IsAny<GridRequest>(), null))
                          .ReturnsAsync(pagedList);

            var result = await _service.GetPagedBooks(gridRequest);

            result.TotalCount.Should().Be(totalCount);
            result.NumberOfPages.Should().Be(expectedPages);
            result.Items.Should().BeEquivalentTo(items, options => options.ExcludingMissingMembers());
            _mockBooksRepo.Verify(
                b => b.GetPage(It.Is<GridRequest>(g => g.Skip == skip && g.Take == take), null), Times.Once);
        }
    }
}