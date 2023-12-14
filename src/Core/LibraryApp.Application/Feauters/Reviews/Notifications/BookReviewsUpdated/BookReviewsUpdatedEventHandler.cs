using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;

public class BookReviewsUpdatedEventHandler : INotificationHandler<BookReviewsUpdatedEvent>
{
	private readonly ILibraryDbContext _dbContext;

	public BookReviewsUpdatedEventHandler(ILibraryDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task Handle(BookReviewsUpdatedEvent notification, CancellationToken cancellationToken)
	{
		var book = await _dbContext.Books
			.Include(book => book.Reviews)
			.FirstOrDefaultAsync(book => book.Id == notification.BookId, cancellationToken);

		if (book == null) throw new EntityNotFoundException(nameof(Book), notification.BookId);

		book.Rating = book.Reviews.Any() ? 
			book.Reviews.Average(book => book.Rating) : 0;

		await _dbContext.SaveChangesAsync(cancellationToken);
	}
}
