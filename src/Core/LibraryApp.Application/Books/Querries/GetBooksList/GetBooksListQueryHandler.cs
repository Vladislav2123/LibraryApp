using AutoMapper;
using LibraryApp.Application.Books.Querries.Dto;
using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Application.Books.Querries.GetBooksList
{
    public class GetBooksListQueryHandler : IRequestHandler<GetBooksListQuery, PagedList<BookLookupDto>>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetBooksListQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
			_mapper = mapper;
        }

        public async Task<PagedList<BookLookupDto>> Handle(GetBooksListQuery query, CancellationToken cancellationToken)
		{
			IQueryable<Book> booksQuery = _dbContext.Books.Include(book => book.ReadUsers);
			
			if(string.IsNullOrWhiteSpace(query.SearchTerms) == false)
			{
				booksQuery = booksQuery
					.Where(book => book.Name.Contains(query.SearchTerms));
			}

			var sortingColumnPropertyExpression = GetSortingColumnProperty(query);
			if (query.SortOrder?.ToLower() == "asc")
			{
				booksQuery.OrderBy(sortingColumnPropertyExpression);
			}
			else booksQuery.OrderByDescending(sortingColumnPropertyExpression);

			var booksLookupsQuery = _mapper.Map<IQueryable<BookLookupDto>>(booksQuery);
			var books = await PagedList<BookLookupDto>.CreateAsync(booksLookupsQuery, query.Page, query.PageSize, cancellationToken);

			return books;
		}

		private Expression<Func<Book, object>> GetSortingColumnProperty(GetBooksListQuery query)
		{
			Expression<Func<Book, object>> expression = query.SortColumn?.ToLower() switch
			{
				"name" => book => book.Name,
				"year" => book => book.Year,
				_ => book => book.ReadUsers.Count,
			};

			return expression;
		}
	}
}
