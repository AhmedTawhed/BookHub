using BookHub.Core.DTOs.ReviewDtos;
using BookHub.Core.Entities;
using BookHub.Core.Exceptions;
using BookHub.Core.Interfaces;
using BookHub.Core.Interfaces.Repository;
using BookHub.Infrastructure.Services;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace BookHub.Tests.Services
{
    public class ReviewServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUoW;
        private readonly Mock<IReviewRepository> _mockReviewRepo;
        private readonly IMemoryCache _cache;
        private readonly ReviewService _service;

        public ReviewServiceTests()
        {
            _mockReviewRepo = new Mock<IReviewRepository>();
            _mockUoW = new Mock<IUnitOfWork>();
            _mockUoW.Setup(u => u.Reviews).Returns(_mockReviewRepo.Object);
            _cache = new MemoryCache(new MemoryCacheOptions());
            _service = new ReviewService(_mockUoW.Object, _cache);
        }

        [Fact]
        public async Task GetReviewsByUser_ReturnsReviews()
        {
            var userId = "user-1";
            var reviews = new List<Review>
            {
                new Review { Id = 1, UserId = userId, BookId = 1, Rating = 4, Comment = "Good" },
                new Review { Id = 2, UserId = userId, BookId = 2, Rating = 5, Comment = "Great" }
            };
            _mockReviewRepo.Setup(r => r.GetReviewsByUserId(userId)).ReturnsAsync(reviews);

            var result = await _service.GetReviewsByUser(userId);

            result.Should().HaveCount(2);
            result.First().UserId.Should().Be(userId);
            _mockReviewRepo.Verify(r => r.GetReviewsByUserId(userId), Times.Once);
        }

        [Fact]
        public async Task GetReviewsByBook_ReturnsReviews()
        {
            var bookId = 1;
            var reviews = new List<Review>
            {
                new Review { Id = 1, UserId = "user-1", BookId = bookId, Rating = 3 },
                new Review { Id = 2, UserId = "user-2", BookId = bookId, Rating = 5 }
            };
            _mockReviewRepo.Setup(r => r.GetReviewsByBookId(bookId)).ReturnsAsync(reviews);

            var result = await _service.GetReviewsByBook(bookId);

            result.Should().HaveCount(2);
            result.All(r => r.BookId == bookId).Should().BeTrue();
            _mockReviewRepo.Verify(r => r.GetReviewsByBookId(bookId), Times.Once);
        }

        [Fact]
        public async Task GetReviewById_ReviewExists_ReturnsDto()
        {
            var review = new Review { Id = 1, UserId = "user-1", BookId = 1, Rating = 4, Comment = "Nice", CreatedAt = DateTime.UtcNow };
            _mockReviewRepo.Setup(r => r.GetById(1)).ReturnsAsync(review);

            var result = await _service.GetReviewById(1);

            result.Should().NotBeNull();
            result.Id.Should().Be(1);
            result.Rating.Should().Be(4);
            result.Comment.Should().Be("Nice");
        }

        [Fact]
        public async Task GetReviewById_ReviewDoesNotExist_ThrowsNotFoundException()
        {
            _mockReviewRepo.Setup(r => r.GetById(99)).ReturnsAsync((Review?)null);

            Func<Task> act = async () => await _service.GetReviewById(99);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Review not found.");
        }

        [Fact]
        public async Task AddReview_ValidRequest_AddsReviewAndReturnsDto()
        {
            var dto = new ReviewRequestDto { UserId = "user-1", BookId = 1, Rating = 5, Comment = "Excellent" };
            _mockReviewRepo.Setup(r => r.IsReviewed(dto.UserId, dto.BookId)).ReturnsAsync(false);

            var result = await _service.AddReview(dto);

            result.Should().NotBeNull();
            result.UserId.Should().Be(dto.UserId);
            result.BookId.Should().Be(dto.BookId);
            result.Rating.Should().Be(dto.Rating);
            result.Comment.Should().Be(dto.Comment);
            _mockReviewRepo.Verify(r => r.Add(It.Is<Review>(rv => rv.UserId == dto.UserId && rv.BookId == dto.BookId)), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task AddReview_AlreadyReviewed_ThrowsBadRequestException()
        {
            var dto = new ReviewRequestDto { UserId = "user-1", BookId = 1, Rating = 3 };
            _mockReviewRepo.Setup(r => r.IsReviewed(dto.UserId, dto.BookId)).ReturnsAsync(true);

            Func<Task> act = async () => await _service.AddReview(dto);

            await act.Should().ThrowAsync<BadRequestException>()
                .WithMessage("You have already reviewed this book.");
            _mockReviewRepo.Verify(r => r.Add(It.IsAny<Review>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateReview_ValidRequest_UpdatesAndReturnsDto()
        {
            var userId = "user-1";
            var existing = new Review { Id = 1, UserId = userId, BookId = 1, Rating = 2, Comment = "Meh" };
            var dto = new ReviewRequestDto { UserId = userId, BookId = 1, Rating = 5, Comment = "Changed my mind!" };
            _mockReviewRepo.Setup(r => r.GetById(1)).ReturnsAsync(existing);

            var result = await _service.UpdateReview(userId, 1, dto);

            result.Rating.Should().Be(5);
            result.Comment.Should().Be("Changed my mind!");
            _mockReviewRepo.Verify(r => r.Update(existing), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateReview_ReviewNotFound_ThrowsNotFoundException()
        {
            _mockReviewRepo.Setup(r => r.GetById(99)).ReturnsAsync((Review?)null);

            Func<Task> act = async () => await _service.UpdateReview("user-1", 99, new ReviewRequestDto());

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Review not found.");
            _mockReviewRepo.Verify(r => r.Update(It.IsAny<Review>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task UpdateReview_WrongUser_ThrowsUnauthorizedException()
        {
            var existing = new Review { Id = 1, UserId = "owner-user", BookId = 1, Rating = 3 };
            _mockReviewRepo.Setup(r => r.GetById(1)).ReturnsAsync(existing);

            Func<Task> act = async () => await _service.UpdateReview("different-user", 1, new ReviewRequestDto());

            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("You can only update your own reviews.");
            _mockReviewRepo.Verify(r => r.Update(It.IsAny<Review>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteReview_ValidRequest_DeletesReview()
        {
            var userId = "user-1";
            var review = new Review { Id = 1, UserId = userId, BookId = 1, Rating = 4 };
            _mockReviewRepo.Setup(r => r.GetById(1)).ReturnsAsync(review);

            await _service.DeleteReview(userId, 1);

            _mockReviewRepo.Verify(r => r.Delete(review), Times.Once);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteReview_ReviewNotFound_ThrowsNotFoundException()
        {
            _mockReviewRepo.Setup(r => r.GetById(99)).ReturnsAsync((Review?)null);

            Func<Task> act = async () => await _service.DeleteReview("user-1", 99);

            await act.Should().ThrowAsync<NotFoundException>()
                .WithMessage("Review not found.");
            _mockReviewRepo.Verify(r => r.Delete(It.IsAny<Review>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task DeleteReview_WrongUser_ThrowsUnauthorizedException()
        {
            var review = new Review { Id = 1, UserId = "owner-user", BookId = 1, Rating = 3 };
            _mockReviewRepo.Setup(r => r.GetById(1)).ReturnsAsync(review);

            Func<Task> act = async () => await _service.DeleteReview("different-user", 1);

            await act.Should().ThrowAsync<UnauthorizedException>()
                .WithMessage("You can only delete your own reviews.");
            _mockReviewRepo.Verify(r => r.Delete(It.IsAny<Review>()), Times.Never);
            _mockUoW.Verify(u => u.CompleteAsync(), Times.Never);
        }
    }
}
