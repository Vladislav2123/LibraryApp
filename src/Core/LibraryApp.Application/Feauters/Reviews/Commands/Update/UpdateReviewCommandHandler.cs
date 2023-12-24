using LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Update;

public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IPublisher _publisher;

	public UpdateReviewCommandHandler(ILibraryDbContext dbContext, IPublisher publisher)
	{
		_dbContext = dbContext;
		_publisher = publisher;
	}

	public async Task<Unit> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
	{
		var review = await _dbContext.Reviews
			.FirstOrDefaultAsync(review => review.Id == command.ReviewId, cancellationToken);

		if (review == null) throw new EntityNotFoundException(nameof(Review), command.ReviewId);

		review.Rating = command.Rating;
		review.Title = command.Title;
		review.Comment = command.Comment;

		await _dbContext.SaveChangesAsync(cancellationToken);

		await _publisher.Publish(new BookReviewsUpdatedEvent(review.BookId));

		return Unit.Value;
	}
}
