using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Features.Users.Commands.DeleteReadBook;

public class DeleteReadBookCommandHandler : IRequestHandler<DeleteReadBookCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;

	public DeleteReadBookCommandHandler(ILibraryDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(DeleteReadBookCommand command, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users
			.Include(user => user.ReadBooks)
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

		var book = user.ReadBooks.FirstOrDefault(book => book.Id == command.BookId);

		if (book == null) throw new UserHasNotReadBookException(command.UserId, command.BookId);

		user.ReadBooks.Remove(book);
		book.Readers.Remove(user);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
