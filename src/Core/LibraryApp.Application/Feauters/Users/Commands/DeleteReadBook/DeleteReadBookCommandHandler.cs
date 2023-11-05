using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Users.Commands.DeleteReadBook
{
	public class DeleteReadBookCommandHandler : IRequestHandler<DeleteReadBookCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public DeleteReadBookCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(DeleteReadBookCommand request, CancellationToken cancellationToken)
		{
			var user = await _dbContext.Users
				.Include(user => user.ReadBooks)
				.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

			if (user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

			var book = user.ReadBooks.FirstOrDefault(book => book.Id == request.BookId);

			if (book == null) throw new UserHasNotReadBookException(request.UserId, request.BookId);

			user.ReadBooks.Remove(book);
			book.Readers.Remove(user);

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
