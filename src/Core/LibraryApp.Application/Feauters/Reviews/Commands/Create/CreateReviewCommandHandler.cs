using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Reviews.Notifications.BookReviewsUpdated;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create;

    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, Guid>
    {
        private readonly ILibraryDbContext _dbContext;
        private readonly IPublisher _publisher;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private HttpContext HttpContext => _httpContextAccessor.HttpContext;

	public CreateReviewCommandHandler(
            ILibraryDbContext dbContext, 
            IPublisher publisher, 
            IHttpContextAccessor httpContextAccessor)
	{
		_dbContext = dbContext;
		_publisher = publisher;
		_httpContextAccessor = httpContextAccessor;
	}

	public async Task<Guid> Handle(CreateReviewCommand command, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books
                .Include(book => book.Reviews)
                .FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

            if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

            Guid userId = Guid.Parse(HttpContext.User.FindFirstValue(ClaimTypes.Actor));

            if(book.Reviews
                .Any(review => review.UserId == userId))
            {
                throw new BookAlreadyHasReviewException(command.BookId, userId);
            }

            var newReview = new Review
            {
                Id = Guid.NewGuid(),
                UserId = userId,
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
