using LibraryApp.Domain.Models;
using MediatR;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Books.Querries.GetBookContent;

public class GetBookContentQueryHandler : IRequestHandler<GetBookContentQuery, FileVm>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IContentTypeProvider _contentTypeProvider;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public GetBookContentQueryHandler(
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

	public async Task<FileVm> Handle(GetBookContentQuery request, CancellationToken cancellationToken)
	{
		var book = await _cache
			.GetAsync(
				$"{_cacheKeys.Book}{request.BookId}",
				BookQuery,
				cancellationToken
			);

		string contentType;
		if (_contentTypeProvider.TryGetContentType(book.ContentPath, out contentType) == false)
			throw new ContentTypeNotFoundException($"Book Content");

		return new FileVm
		{
			FileName = $"{book.Name}{Path.GetExtension(book.ContentPath)}",
			ContentType = contentType,
			Bytes = await File.ReadAllBytesAsync(book.ContentPath, cancellationToken)
		};

		async Task<Book> BookQuery()
		{
			var book = await _dbContext.Books
			.AsNoTracking()
			.Select(book => new Book
			{
				Id = book.Id,
				ContentPath = book.ContentPath
			})
			.FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

			return book;
		}
	}
}
