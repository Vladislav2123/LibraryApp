using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Books.Commands.Update;
public class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public UpdateBookCommandHandler(
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

	public async Task<Unit> Handle(UpdateBookCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Book}{command.BookId}";

		var book = await _cache
			.GetAsync(
				cacheKey, 
				BookQuery, 
				cancellationToken);

		if (await _dbContext.Authors
			.AnyAsync(author => author.Id == command.AuthorId, cancellationToken) == false)
			throw new EntityNotFoundException(nameof(Author), command.AuthorId);

		var sameBook = await _dbContext.Books
			.FirstOrDefaultAsync(book =>
				book.AuthorId == command.AuthorId &&
				book.Name == command.Name &&
				book.Description == command.Description &&
				book.Year == command.Year, cancellationToken);


		if (sameBook != null)
		{
			// If there is other book with the same properties, EntityAlreadyExist trowing
			if (sameBook.Id != book.Id) throw new EntityAlreadyExistException(nameof(Book));

			// If this book has the same properties and content file don`t updating, EntityHasNoChanges trowing
			if (command.ContentFile == null) throw new EntityHasNoChangesException(nameof(Book), command.BookId);
		}

		book.AuthorId = command.AuthorId;
		book.Name = command.Name;
		book.Description = command.Description;
		book.Year = command.Year;

		if (command.ContentFile != null)
		{
			_fileWrapper.DeleteFile(book.ContentPath);
			await _fileWrapper.SaveFileAsync(command.ContentFile, book.ContentPath, cancellationToken);
		}

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
