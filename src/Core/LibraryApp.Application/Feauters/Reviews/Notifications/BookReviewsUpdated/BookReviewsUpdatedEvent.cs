using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated
{
    public record BookReviewsUpdatedEvent(Guid BookId) : INotification;
}
