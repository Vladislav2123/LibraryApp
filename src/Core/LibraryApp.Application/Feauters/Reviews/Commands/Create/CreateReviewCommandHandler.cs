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
            var book = await _dbContext.Books
                .Include(book => book.Reviews)
                .FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

            if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

            if (await _dbContext.Users.AnyAsync(user => 
                user.Id == command.UserId, cancellationToken) == false)
            {
                throw new EntityNotFoundException(nameof(User), command.UserId);
            }

            if(book.Reviews.Any(review => 
                review.UserId == command.UserId))
            {
                throw new BookAlreadyHasReviewException(command.BookId, command.UserId);
            }

            var newReview = new Review
            {
                Id = Guid.NewGuid(),
                UserId = command.UserId,
                BookId = command.BookId,
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
