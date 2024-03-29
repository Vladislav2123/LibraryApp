﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Reviews.Commands.Delete;

public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IPublisher _publisher;

	public DeleteReviewCommandHandler(ILibraryDbContext dbContext, IPublisher publisher)
	{
		_dbContext = dbContext;
		_publisher = publisher;
	}

	public async Task<Unit> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
	{
		var review = await _dbContext.Reviews
			.FirstOrDefaultAsync(review => review.Id == command.ReviewId, cancellationToken);

		if (review == null) throw new EntityNotFoundException(nameof(Review), command.ReviewId);

		_dbContext.Reviews.Remove(review);
		await _dbContext.SaveChangesAsync(cancellationToken);

		await _publisher.Publish(new BookReviewsUpdatedEvent(review.BookId));

		return Unit.Value;
	}
}
