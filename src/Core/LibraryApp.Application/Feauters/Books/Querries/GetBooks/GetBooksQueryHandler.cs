using AutoMapper;
using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryApp.Application.Feauters.Books.Querries.Dto;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBooks
{
    public class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, PagedList<BookLookupDto>>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetBooksQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
			_mapper = mapper;
        }

        public async Task<PagedList<BookLookupDto>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
		{
			IQueryable<Book> booksQuery = _dbContext.Books.Include(book => book.ReadUsers);
			
			if(string.IsNullOrWhiteSpace(request.SearchTerms) == false)
			{
				booksQuery = booksQuery
					.Where(book => book.Name.Contains(request.SearchTerms));
			}
			if(request.AuthorId != null && request.AuthorId != Guid.Empty)
			{
				booksQuery = booksQuery
					.Where(book => book.AuthorId == request.AuthorId);
			}

			var sortingColumnPropertyExpression = GetSortingColumnProperty(request);
			if (request.SortOrder?.ToLower() == "asc")
			{
				booksQuery.OrderBy(sortingColumnPropertyExpression);
			}
			else booksQuery.OrderByDescending(sortingColumnPropertyExpression);

			var booksLookups = _mapper.Map<List<BookLookupDto>>(booksQuery.ToListAsync(cancellationToken));
			return PagedList<BookLookupDto>.Create(booksLookups, request.Page, request.PageSize);
		}

		private Expression<Func<Book, object>> GetSortingColumnProperty(GetBooksQuery request)
		{
			Expression<Func<Book, object>> expression = request.SortColumn?.ToLower() switch
			{
				"name" => book => book.Name,
				"year" => book => book.Year,
				_ => book => book.ReadUsers.Count,
			};

			return expression;
		}
	}
}
