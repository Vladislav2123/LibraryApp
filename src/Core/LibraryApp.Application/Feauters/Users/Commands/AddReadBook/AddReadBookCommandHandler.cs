using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Users.Commands.AddReadedBook
{
	public class AddReadBookCommandHandler : IRequestHandler<AddReadBookCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public AddReadBookCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(AddReadBookCommand request, CancellationToken cancellationToken)
		{
			var user = await _dbContext.Users
				.Include(user => user.ReadBooks)
				.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

			if(user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

			var book = await _dbContext.Books
				.Include(book => book.Readers)
				.FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

			if(book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

			if (user.ReadBooks.Any(book =>
				book.Id == request.BookId))
			{
				throw new UserAlreadyReadBookException(request.UserId, request.BookId);
			}

			user.ReadBooks.Add(book);
			book.Readers.Add(user);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
