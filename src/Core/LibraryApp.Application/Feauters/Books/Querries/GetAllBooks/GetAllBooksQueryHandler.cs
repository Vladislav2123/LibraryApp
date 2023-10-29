using AutoMapper;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryApp.Application.Feauters.Books.Querries.Dto;
using LibraryApp.Application.Common.Pagination;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBooks
{
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
				.Include(book => book.Readers);
			
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

			var booksLookups = _mapper.Map<List<BookLookupDto>>(await booksQuery.ToListAsync(cancellationToken));
			return PagedList<BookLookupDto>.Create(booksLookups, request.Page);
		}

		private Expression<Func<Book, object>> GetSortingColumnProperty(GetAllBooksQuery request)
		{
			Expression<Func<Book, object>> expression = request.SortColumn?.ToLower() switch
			{
				"name" => book => book.Name,
				"year" => book => book.Year,
				_ => book => book.Readers.Count,
			};

			return expression;
		}
	}
}
