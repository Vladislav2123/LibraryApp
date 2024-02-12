using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Domain.Exceptions.FileNotFoundException;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Books.Commands.DeleteBookCover;

public class DeleteBookCoverCommandHandler : IRequestHandler<DeleteBookCoverCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public DeleteBookCoverCommandHandler(
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

	public async Task<Unit> Handle(DeleteBookCoverCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Book}{command.BookId}";

		var book = await _cache
			.GetAsync(
				cacheKey,
				BookQuery,
				cancellationToken
			);

		if (string.IsNullOrEmpty(book.CoverPath) ||
			Path.Exists(book.CoverPath) == false)
			throw new FileNotFoundException("Book cover");

		_fileWrapper.DeleteFile(book.CoverPath);
		book.CoverPath = string.Empty;

		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.SetAsync(cacheKey, book, cancellationToken);

		return Unit.Value;

		async Task<Book> BookQuery()
		{
			var book = await _dbContext.Books
			.FirstOrDefaultAsync(book => book.Id == command.BookId, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), command.BookId);

			return book;
		}
	}
}
