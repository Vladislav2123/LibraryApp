﻿using LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Application.Feauters.Reviews.Commands.Update;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Enteties;
using Moq.EntityFrameworkCore;
using FluentAssertions;
using MediatR;
using Moq;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using LibraryApp.Application.Common.Exceptions;

namespace LibraryApp.Tests.ReviewTests;
public class UpdateReviewTests
{
	private readonly Mock<ILibraryDbContext> _dbContextMock =
		new Mock<ILibraryDbContext>();

	private readonly Mock<IPublisher> _publisherMock =
		new Mock<IPublisher>();

	[Fact]
	public async Task Handle_ExpectedBehavior_ReturnUnit()
	{
		// Arrange
		var review = new Review()
		{
			Id = Guid.NewGuid(),
			BookId = Guid.NewGuid(),
			Rating = 1,
			Title = "Title",
			Text = "Comment"
		};

		_dbContextMock
			.Setup(x => x.Reviews)
			.ReturnsDbSet(new Review[] { review });

		var command = new UpdateReviewCommand(
			review.Id, 2, "NewTitle", "NewComment");

		var handler = new UpdateReviewCommandHandler(
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

	[Fact]
	public async Task Handle_NonexistentReview_ThrowEntityNotFoundException()
	{
		// Arrange
		_dbContextMock
			.Setup(x => x.Reviews)
			.ReturnsDbSet(new Review[0]);

		var command = new UpdateReviewCommand(
			Guid.NewGuid(), 1, "Title", "Comment");

		var handler = new UpdateReviewCommandHandler(
			_dbContextMock.Object,
			_publisherMock.Object);

		// Act
		var action = async () =>
			await handler.Handle(command, CancellationToken.None);

		// Assert
		await action.Should().ThrowAsync<EntityNotFoundException>();
	}
}
