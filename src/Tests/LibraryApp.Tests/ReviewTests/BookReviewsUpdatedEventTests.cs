using LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Enteties;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using Moq;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Reviews.Notifications.BookReviewsUpdated;

namespace LibraryApp.Tests.ReviewTests;
public class BookReviewsUpdatedEventTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock = new();

	[Fact]
	public async Task Handle_ExpectedBehavior()
	{
		// Arrange
		var book = new Book
		{
			Id = Guid.NewGuid(),
			Reviews = new List<Review>
			{
				new Review { Rating = 4 },
				new Review { Rating = 5 },
			}
		};

		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[] { book });

		var notification = new BookReviewsUpdatedEvent(book.Id);

		var handler = new BookReviewsUpdatedEventHandler(
			_dbContextMock.Object);

		// Act
		await handler.Handle(notification, CancellationToken.None);

		// Assert
		var updatedBook = _dbContextMock.Object.Books.FirstOrDefault();

		updatedBook.Rating.Should().Be(4.5);

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));
	}

	[Fact]
	public async Task Handle_NonexistentBook_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Books)
			.ReturnsDbSet(new Book[0]);

		var notification = new BookReviewsUpdatedEvent(Guid.NewGuid());

		var handler = new BookReviewsUpdatedEventHandler(
			_dbContextMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(notification, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
