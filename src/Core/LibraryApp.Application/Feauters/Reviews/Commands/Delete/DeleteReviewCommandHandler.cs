using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Delete
{
	public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public DeleteReviewCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(DeleteReviewCommand command, CancellationToken cancellationToken)
		{
			var review = await _dbContext.Reviews.FirstOrDefaultAsync(review => review.Id == command.Id, cancellationToken);

			if (review == null) throw new EntityNotFoundException(nameof(Review), command.Id);

			_dbContext.Reviews.Remove(review);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
