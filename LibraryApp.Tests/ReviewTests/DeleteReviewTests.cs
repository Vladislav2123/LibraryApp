﻿using LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Application.Feauters.Reviews.Commands.Delete;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Enteties;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using MediatR;
using Moq;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Reviews.Commands.Update;

namespace LibraryApp.Tests.ReviewTests;
public class DeleteReviewTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();

	private readonly Mock<IPublisher> _publisherMock =
		new Mock<IPublisher>();

	[Fact]
	public async Task Handle_ExpectedBehavior_ReturnUnit()
	{
		// Arrange
		var review = new Review() { Id = Guid.NewGuid() };

		_dbContextMock
			.Setup(x => x.Reviews)
			.ReturnsDbSet(new Review[] { review });

		var command = new DeleteReviewCommand(review.Id);

		var handler = new DeleteReviewCommandHandler(
			_dbContextMock.Object,
			_publisherMock.Object);
		// Act
		var result = await handler.Handle(command, CancellationToken.None);

		// Assert
		result.Should().NotBeNull();

		_dbContextMock.Verify(x =>
			x.SaveChangesAsync(CancellationToken.None));

		_publisherMock.Verify(x =>
			x.Publish(new BookReviewsUpdatedEvent(review.BookId), CancellationToken.None));
	}

	public async Task Handle_NonexistentReview_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Reviews)
			.ReturnsDbSet(new Review[0]);

		var command = new DeleteReviewCommand(Guid.NewGuid());

		var handler = new DeleteReviewCommandHandler(
			_dbContextMock.Object,
			_publisherMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}