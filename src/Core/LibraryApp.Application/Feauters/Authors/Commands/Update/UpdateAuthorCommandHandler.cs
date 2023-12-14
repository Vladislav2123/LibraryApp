using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Authors.Commands.Update;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;

	public UpdateAuthorCommandHandler(ILibraryDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
	{
		var author = await _dbContext.Authors
			.FirstOrDefaultAsync(author => author.Id == command.AuthorId, cancellationToken);

		if (author == null) throw new EntityNotFoundException(nameof(Author), command.AuthorId);

		var sameAuthor = await _dbContext.Authors
			.FirstOrDefaultAsync(author =>
				author.Name == command.Name &&
				author.BirthDate == command.BirthDate, cancellationToken);

		if(sameAuthor != null)
		{
			if(sameAuthor.Id == author.Id) 
				throw new EntityHasNoChangesException(nameof(Author), command.AuthorId);
			else throw new EntityAlreadyExistException(nameof(Author));
		}

		author.Name = command.Name;
		author.BirthDate = command.BirthDate;

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
