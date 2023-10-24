using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Authors.Commands.Delete
{
	public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public DeleteAuthorCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
		{
			var author = await _dbContext.Authors.FirstOrDefaultAsync(author => author.Id == command.Id, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), command.Id);

			_dbContext.Authors.Remove(author);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
