using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Authors.Commands.Update
{
	public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public UpdateAuthorCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
		{
			var author = await _dbContext.Authors.FirstOrDefaultAsync(author => author.Id == command.Id);
			if (author == null) throw new EntityNotFoundException(nameof(Author), command.Id);

			author.Name = command.Name;
			author.BirthDate = command.BirthDate;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
