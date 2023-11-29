using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Books.Commands.Delete;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;

namespace LibraryApp.Application.Feauters.Books.Commands.DeleteBookCover
{
	public class DeleteBookCoverCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
	{
		private readonly ILibraryDbContext _dbContext;

		public DeleteBookCoverCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Unit> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
		{
			var book = await _dbContext.Books
				.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

			if (string.IsNullOrEmpty(book.CoverPath) ||
				Path.Exists(book.CoverPath) == false)
				throw new FileNotFoundException("Book cover");

			File.Delete(book.CoverPath);
			book.CoverPath = string.Empty;

			await _dbContext.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}
