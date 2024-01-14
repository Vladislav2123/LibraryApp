using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Users.Commands.AddReadBook;

public class AddReadBookCommandHandler : IRequestHandler<AddReadBookCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;

	public AddReadBookCommandHandler(ILibraryDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(AddReadBookCommand command, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users
			.Include(user => user.ReadBooks)
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

		var book = await _dbContext.Books
			.Include(book => book.Readers)
			.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

		if(book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

		if (user.ReadBooks.Any(book =>
			book.Id == command.BookId))
		{
			throw new UserAlreadyReadBookException(command.UserId, command.BookId);
		}

		user.ReadBooks.Add(book);
		book.Readers.Add(user);

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
