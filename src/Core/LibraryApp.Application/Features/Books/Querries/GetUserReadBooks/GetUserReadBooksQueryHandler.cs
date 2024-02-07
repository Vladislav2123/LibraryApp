using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Books.Querries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Books.Querries.GetUserReadBooks;

public class GetUserReadBooksQueryHandler : IRequestHandler<GetUserReadBooksQuery, PagedList<BookLookupDto>>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetUserReadBooksQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PagedList<BookLookupDto>> Handle(GetUserReadBooksQuery request, CancellationToken cancellationToken)
	{
		User user = await _dbContext.Users
			.AsNoTracking()
			.Include(user => user.ReadBooks)
			.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

		if (user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

		var books = user.ReadBooks
			.Skip((request.Page.number - 1) * request.Page.size)
			.Take(request.Page.size)
			.ToList();
		var mappedBooks = _mapper.Map<List<BookLookupDto>>(user.ReadBooks);

		return new PagedList<BookLookupDto>(mappedBooks, user.ReadBooks.Count, request.Page);
	}
}
