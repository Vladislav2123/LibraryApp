using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
    {
        private readonly ILibraryDbContext _dbContext;
        private readonly IPublisher _publisher;

        public CreateReviewCommandHandler(ILibraryDbContext dbContext, IPublisher publisher)
        {
            _dbContext = dbContext;
            _publisher = publisher;
        }

        public async Task<Guid> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);
            if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);
            if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

            var newReview = new Review
            {
                Id = Guid.NewGuid(),
                UserId = user.Id,
                BookId = book.Id,
                Rating = command.Rating,
                Title = command.Title,
                Text = command.Text,
                CreationDate = DateTime.Now,
            };
            
            await _dbContext.Reviews.AddAsync(newReview, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            await _publisher.Publish(new BookReviewsUpdatedEvent(command.BookId));

            return newReview.Id;
        }
    }
}
