using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Authors.Commands.Delete;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;

	public DeleteAuthorCommandHandler(ILibraryDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
	{
		var author = await _dbContext.Authors
			.FirstOrDefaultAsync(author => author.Id == command.AuthorId, cancellationToken);

		if (author == null) throw new EntityNotFoundException(nameof(Author), command.AuthorId);

		if (string.IsNullOrEmpty(author.AvatarPath) == false)
			File.Delete(author.AvatarPath);

		_dbContext.Authors.Remove(author);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
