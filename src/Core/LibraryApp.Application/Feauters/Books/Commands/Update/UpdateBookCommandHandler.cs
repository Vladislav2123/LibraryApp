using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Books.Commands.Update
{
	public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public UpdateBookCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
		{
			var book = await _dbContext.Books
				.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

			if(book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

			if (await _dbContext.Authors
				.AnyAsync(author => author.Id == command.AuthorId, cancellationToken) == false)
			{
				throw new EntityNotFoundException(nameof(Author), command.AuthorId);
			}

			var sameBook = await _dbContext.Books
				.FirstOrDefaultAsync(book =>
					book.AuthorId == command.AuthorId &&
					book.Name == command.Name &&
					book.Description == command.Description &&
					book.Year == command.Year, cancellationToken);

			if(sameBook != null)
			{
				if(sameBook.Id == book.Id) 
					throw new EntityHasNoChangesException(nameof(Book), command.BookId);
				else throw new EntityAlreadyExistException(nameof(Book));
			}

			book.AuthorId = command.AuthorId;
			book.Name = command.Name;
			book.Description = command.Description;
			book.Year = command.Year;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
