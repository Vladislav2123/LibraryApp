using LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Application.Feauters.Reviews.Commands.Create;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Enteties;
using Microsoft.AspNetCore.Http;
using LibraryApp.Tests.Common;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using MediatR;
using Moq;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Reviews.Commands.Create;
using LibraryApp.Application.Features.Reviews.Notifications.BookReviewsUpdated;

namespace LibraryApp.Tests.ReviewTests;
public class CreateReviewTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();
	private readonly Mock<IPublisher> _publisherMock = new();
	private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();

	[Fact]
	public async Task Handler_ExpectedBehavior_ReturnCreatedReviewGuid()
	{
		// Arrange
		var book = new Book
		{
			Id = Guid.NewGuid(),
			Reviews = new List<Review>()
		};

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		_dbContextMock
			.Setup(x => x.Reviews)
			.ReturnsDbSet(new Review[0]);

		_httpContextAccessorMock
			.Setup(x => x.HttpContext)
			.Returns(TestingHelper.GetHttpContextWithActorClaim(
				Guid.NewGuid().ToString()));

		var command = new CreateReviewCommand(
			book.Id, 5, "Title", "Comment");

		var handler = new CreateReviewCommandHandler(
			_dbContextMock.Object,
			_publisherMock.Object,
			_httpContextAccessorMock.Object);

		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeEmpty();

		_dbContextMock.Verify(x =>
			x.Reviews.AddAsync(It.IsAny<Review>(), CancellationToken.None));

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));

		_publisherMock.Verify(x =>
			x.Publish(new BookReviewsUpdatedEvent(book.Id), CancellationToken.None));
	}

	[Fact]
	public async Task Handler_NonexistentBook_ThrowEntityNotFoundExeption()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[0]);

		_dbContextMock
			.Setup(x => x.Reviews)
			.ReturnsDbSet(new Review[0]);

		_httpContextAccessorMock
			.Setup(x => x.HttpContext)
			.Returns(TestingHelper.GetHttpContextWithActorClaim(
				Guid.NewGuid().ToString()));

		var command = new CreateReviewCommand(
			Guid.NewGuid(), 5, "Title", "Comment");

		var handler = new CreateReviewCommandHandler(
			_dbContextMock.Object,
			_publisherMock.Object,
			_httpContextAccessorMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}


	[Fact]
	public async Task Handle_ReviewAlreadyExist_ThrowBookAlreadyHasReviewException()
	{
		// Arrange
		var userId = Guid.NewGuid();

		var book = new Book
		{
			Id = Guid.NewGuid(),
			Reviews = new List<Review>()
			{
				new Review { UserId = userId }
			}
		};

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		_httpContextAccessorMock
			.Setup(x => x.HttpContext)
			.Returns(TestingHelper.GetHttpContextWithActorClaim(
				userId.ToString()));

		var command = new CreateReviewCommand(
			book.Id, 5, "Title", "Comment");

		var handler = new CreateReviewCommandHandler(
			_dbContextMock.Object,
			_publisherMock.Object,
			_httpContextAccessorMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<BookAlreadyHasReviewException>();
	}
}
