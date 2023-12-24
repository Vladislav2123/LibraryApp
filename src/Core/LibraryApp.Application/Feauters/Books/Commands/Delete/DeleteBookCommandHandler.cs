using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Feauters.Books.Commands.Delete;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;


	public DeleteBookCommandHandler(ILibraryDbContext dbContext, IFileWrapper fileWrapper)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
	}

	public async Task<Unit> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
	{
		Book book = await _dbContext.Books
			.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

		if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

		_fileWrapper.DeleteFile(book.ContentPath);
		_fileWrapper.DeleteFile(book.CoverPath);

		_dbContext.Books.Remove(book);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
