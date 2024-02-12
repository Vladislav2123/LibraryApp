using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Books.Querries.Dto;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Books.Querries.GetBook;

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

    public GetBookQueryHandler(
        ILibraryDbContext dbContext,
        IMapper mapper,
        ICacheService cache,
        ICacheKeys cacheKeys)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cache = cache;
        _cacheKeys = cacheKeys;
    }

    public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
	{
		var book = await _cache.GetAndSetAsync(
				$"{_cacheKeys.Book}{request.BookId}",
				BookQuery,
				cancellationToken);

		return _mapper.Map<BookDto>(book);

		async Task<Book> BookQuery()
		{
			var book = await _dbContext.Books
				.AsNoTracking()
				.Select(book => new Book
				{
					Id = book.Id,
					CreatedUserId = book.CreatedUserId,
					AuthorId = book.AuthorId,
					Name = book.Name,
					Rating = book.Rating,
					Description = book.Description,
					Year = book.Year
				})
				.FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

			if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

			return book;
		}
	}
}
