using MediatR;

namespace LibraryApp.Application.Features.Reviews.Notifications.BookReviewsUpdated;

public record BookReviewsUpdatedEvent(Guid BookId)
	: INotification;
