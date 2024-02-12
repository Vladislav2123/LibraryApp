using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Domain.Exceptions.FileNotFoundException;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Books.Querries.GetBookCover;

public class GetBookCoverQueryHandler : IRequestHandler<GetBookCoverQuery, FileVm>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IContentTypeProvider _contentTypeProvider;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public GetBookCoverQueryHandler(
		ILibraryDbContext dbContext,
		IContentTypeProvider contentTypeProvider,
		ICacheService cache,
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_contentTypeProvider = contentTypeProvider;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<FileVm> Handle(GetBookCoverQuery request, CancellationToken cancellationToken)
	{
		var book = await _cache
			.GetAsync(
				$"{_cacheKeys.Book}{request.BookId}",
				BookQuery,
				cancellationToken
			);

		if (string.IsNullOrEmpty(book.CoverPath) ||
			Path.Exists(book.CoverPath) == false)
			throw new FileNotFoundException("Book cover");

		string contentType;
		if (_contentTypeProvider.TryGetContentType(book.CoverPath, out contentType) == false)
			throw new ContentTypeNotFoundException("Book Cover");

		return new FileVm()
		{
			FileName = $"{book.Name}_cover{Path.GetExtension(book.CoverPath)}",
			ContentType = contentType,
			Bytes = await File.ReadAllBytesAsync(book.CoverPath, cancellationToken)
		};

		async Task<Book> BookQuery()
		{
			var book = await _dbContext.Books
			.AsNoTracking()
			.Select(book => new Book
			{
				Id = book.Id,
				CoverPath = book.CoverPath
			})
			.FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

			return book;
		}
	}
}
