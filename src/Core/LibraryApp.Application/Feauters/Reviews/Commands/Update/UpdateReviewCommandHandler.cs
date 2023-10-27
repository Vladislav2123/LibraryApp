using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Update
{
	public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public UpdateReviewCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(UpdateReviewCommand command, CancellationToken cancellationToken)
		{
			var review = await _dbContext.Reviews.FirstOrDefaultAsync(review => review.Id == command.ReviewId, cancellationToken);

			if(review == null) throw new EntityNotFoundException(nameof(Review), command.ReviewId);

			review.Rating = command.Rating;
			review.Title = command.Title;
			review.Text = command.Text;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
