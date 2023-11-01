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
			Book book = await _dbContext.Books
				.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

			if(book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

			if (Equal(command, book)) throw new EntityHasNoChangesException(nameof(Book), command.BookId);

			if (_dbContext.Authors.Any(author =>
				author.Id == command.AuthorId) == false)
			{
				throw new EntityNotFoundException(nameof(Author), command.AuthorId);
			}

			if (_dbContext.Books.Any(book =>
				Equal(command, book)))
			{
				throw new EntityAlreadyExistException(nameof(Book));
			}

			book.AuthorId = command.AuthorId;
			book.Name = command.Name;
			book.Description = command.Description;
			book.Year = command.Year;
			book.Text = command.Text;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}

		private bool Equal(UpdateBookCommand command, Book book)
		{
			return
				book.AuthorId == command.AuthorId &&
				book.Name == command.Name &&
				book.Description == command.Description &&
				book.Year == command.Year;
		}
	}
}
