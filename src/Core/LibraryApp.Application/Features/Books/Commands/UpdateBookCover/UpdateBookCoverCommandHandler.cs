using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Books.Commands.UpdateBookCover;

public class UpdateBookCoverCommandHandler : IRequestHandler<UpdateBookCoverCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly FilePaths _filePaths;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public UpdateBookCoverCommandHandler(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper,
		IOptions<FilePaths> filePathsOptions,
		ICacheService cache,
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
		_filePaths = filePathsOptions.Value;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<Unit> Handle(UpdateBookCoverCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Book}{command.BookId}";

		var book = await _cache
			.GetAsync(
				cacheKey,
				BookQuery,
				cancellationToken
			);

		if (string.IsNullOrEmpty(book.CoverPath))
		{
			string newCoverFileName = $"{Guid.NewGuid()}{Path.GetExtension(command.CoverFile.FileName)}";
			book.CoverPath = Path.Combine(_filePaths.CoversPath, newCoverFileName);
			
			await _cache.SetAsync(cacheKey, book, cancellationToken);
		}
		else _fileWrapper.DeleteFile(book.CoverPath);

		await _fileWrapper.SaveFileAsync(command.CoverFile, book.CoverPath, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

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
