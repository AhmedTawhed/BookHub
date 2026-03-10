using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Services;
using FluentAssertions;
using Moq;

namespace BookHub.Tests.Services
{
    public class FavoriteBookServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<IFavoriteBookRepository> _mockFavRepo;
        private readonly Mock<IBookRepository> _mockBooksRepo;
        private readonly FavoriteBookService _service;

        public FavoriteBookServiceTests()
        {
            _mockFavRepo = new Mock<IFavoriteBookRepository>();
            _mockBooksRepo = new Mock<IBookRepository>();
            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.Setup(u => u.FavoriteBooks).Returns(_mockFavRepo.Object);
            _mockUoW.Setup(u => u.Books).Returns(_mockBooksRepo.Object);
            _service = new FavoriteBookService(_mockUoW.Object);
        }

        [Fact]
        public async Task GetUserFavoriteBooks_ReturnsMappedDtos()
        {
            var userId = "user-1";
            var book = new Book { Id = 1, Title = "Clean Code" };
            var favorites = new List<FavoriteBook>
            {
                new FavoriteBook { UserId = userId, BookId = 1, Book = book }
            };
            _mockFavRepo.Setup(r => r.GetFavoriteBooksByUserId(userId)).ReturnsAsync(favorites);

            var result = await _service.GetUserFavoriteBooks(userId);

            result.Should().HaveCount(1);
            result.First().BookId.Should().Be(1);
            result.First().BookTitle.Should().Be("Clean Code");
            result.First().UserId.Should().Be(userId);
        }

        [Fact]
        public async Task AddFavoriteBook_ValidRequest_AddsAndReturnsDto()
        {
            var userId = "user-1";
            var book = new Book { Id = 1, Title = "The Hobbit" };
            _mockFavRepo.Setup(r => r.GetFavoriteBook(userId, 1)).ReturnsAsync((FavoriteBook?)null);
            _mockBooksRepo.Setup(r => r.GetById(1)).ReturnsAsync(book);

            var result = await _service.AddFavoriteBook(userId, 1);

            result.Should().NotBeNull();
            result.BookId.Should().Be(1);
            result.BookTitle.Should().Be("The Hobbit");
            result.UserId.Should().Be(userId);
            _mockFavRepo.Verify(r => r.Add(It.Is<FavoriteBook>(f => f.UserId == userId && f.BookId == 1)), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddFavoriteBook_AlreadyFavorited_ThrowsBadRequestException()
        {
            var userId = "user-1";
            var existing = new FavoriteBook { UserId = userId, BookId = 1 };
            _mockFavRepo.Setup(r => r.GetFavoriteBook(userId, 1)).ReturnsAsync(existing);

            Func<Task> act = async () => await _service.AddFavoriteBook(userId, 1);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("Book already in favorites.");
            _mockFavRepo.Verify(r => r.Add(It.IsAny<FavoriteBook>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task AddFavoriteBook_BookNotFound_ThrowsNotFoundException()
        {
            var userId = "user-1";
            _mockFavRepo.Setup(r => r.GetFavoriteBook(userId, 99)).ReturnsAsync((FavoriteBook?)null);
            _mockBooksRepo.Setup(r => r.GetById(99)).ReturnsAsync((Book?)null);

            Func<Task> act = async () => await _service.AddFavoriteBook(userId, 99);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Book does not exist.");
            _mockFavRepo.Verify(r => r.Add(It.IsAny<FavoriteBook>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task RemoveFavoriteBook_ValidRequest_DeletesAndSaves()
        {
            var userId = "user-1";
            var favorite = new FavoriteBook { UserId = userId, BookId = 1 };
            _mockFavRepo.Setup(r => r.GetFavoriteBook(userId, 1)).ReturnsAsync(favorite);

            await _service.RemoveFavoriteBook(userId, 1);

            _mockFavRepo.Verify(r => r.Delete(favorite), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task RemoveFavoriteBook_NotFound_ThrowsNotFoundException()
        {
            var userId = "user-1";
            _mockFavRepo.Setup(r => r.GetFavoriteBook(userId, 99)).ReturnsAsync((FavoriteBook?)null);

            Func<Task> act = async () => await _service.RemoveFavoriteBook(userId, 99);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Favorite book not found.");
            _mockFavRepo.Verify(r => r.Delete(It.IsAny<FavoriteBook>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
