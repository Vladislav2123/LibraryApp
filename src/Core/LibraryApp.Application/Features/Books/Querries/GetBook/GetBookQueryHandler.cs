using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Books.Querries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Books.Querries.GetBook;

public class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookDto>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetBookQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<BookDto> Handle(GetBookQuery request, CancellationToken cancellationToken)
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

		return _mapper.Map<BookDto>(book);
	}
}
