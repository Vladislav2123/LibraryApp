using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Application.Common.Exceptions.FileNotFoundException;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Books.Commands.DeleteBookCover;

public class DeleteBookCoverCommandHandler : IRequestHandler<DeleteBookCoverCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;

	public DeleteBookCoverCommandHandler(ILibraryDbContext dbContext, IFileWrapper fileWrapper)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
	}

	public async Task<Unit> Handle(DeleteBookCoverCommand command, CancellationToken cancellationToken)
	{
		var book = await _dbContext.Books
			.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

		if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

		if (string.IsNullOrEmpty(book.CoverPath) ||
			Path.Exists(book.CoverPath) == false)
			throw new FileNotFoundException("Book cover");

		_fileWrapper.DeleteFile(book.CoverPath);
		book.CoverPath = string.Empty;

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
