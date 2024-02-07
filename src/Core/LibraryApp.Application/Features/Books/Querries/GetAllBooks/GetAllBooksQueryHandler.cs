using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Books.Querries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Books.Querries.GetAllBooks;

public class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, PagedList<BookLookupDto>>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllBooksQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PagedList<BookLookupDto>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
	{
		IQueryable<Book> booksQuery = _dbContext.Books
			.AsNoTracking()
			.Include(book => book.Readers);

		// Filtering
		if (string.IsNullOrWhiteSpace(request.SearchTerms) == false)
		{
			booksQuery = booksQuery
				.Where(book => book.Name.Contains(request.SearchTerms));
		}
		if (request.AuthorId != null && request.AuthorId != Guid.Empty)
		{
			booksQuery = booksQuery
				.Where(book => book.AuthorId == request.AuthorId);
		}

		// Sorting
		var sortingColumnPropertyExpression = GetSortingColumnProperty(request);
		if (request.SortOrder?.ToLower() == "asc") booksQuery = booksQuery.OrderBy(sortingColumnPropertyExpression);
		else booksQuery = booksQuery.OrderByDescending(sortingColumnPropertyExpression);

		// Response
		var totalAmount = booksQuery.Count();
		var books = booksQuery
			.Skip((request.Page.number - 1) * request.Page.size)
			.Take(request.Page.size)
			.ToList();
		var mappedBooks = _mapper.Map<List<BookLookupDto>>(books);
		return new PagedList<BookLookupDto>(mappedBooks, totalAmount, request.Page);
	}

	private Expression<Func<Book, object>> GetSortingColumnProperty(GetAllBooksQuery request) =>
		request.SortColumn?.ToLower() switch
		{
			"name" => book => book.Name,
			"year" => book => book.Year,
			_ => book => book.Readers.Count,
		};
}
