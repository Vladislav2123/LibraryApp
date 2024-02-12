using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Books.Commands.Delete;

public class DeleteBookCommandHandler : IRequestHandler<DeleteBookCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public DeleteBookCommandHandler(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper,
		ICacheService cache,
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<Unit> Handle(DeleteBookCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Book}{command.BookId}";

		var book = await _cache
			.GetAsync(
				cacheKey,
				BookQuery,
				cancellationToken
			);

		if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

		_fileWrapper.DeleteFile(book.ContentPath);
		_fileWrapper.DeleteFile(book.CoverPath);

		_dbContext.Books.Remove(book);
		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.RemoveAsync(cacheKey);

		return Unit.Value;

		async Task<Book> BookQuery()
		{
			Book book = await _dbContext.Books
			.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

			return book;
		}
	}
}
