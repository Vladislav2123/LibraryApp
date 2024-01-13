using AutoMapper;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Pagination;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetBookReviews;

public class GetBookReviewsQueryHandler : IRequestHandler<GetBookReviewsQuery, PagedList<ReviewDto>>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetBookReviewsQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PagedList<ReviewDto>> Handle(GetBookReviewsQuery request, CancellationToken cancellationToken)
	{
		var book = await _dbContext.Books
			.Include(book => book.Reviews)
			.FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

		if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

		IQueryable<Review> reviewsQuery = book.Reviews.AsQueryable();

		var sortingColumnPropertyExpression = GetSortingColumnProperty(request);
		if (request.SortOrder?.ToLower() == "asc")
		{
			reviewsQuery = reviewsQuery.OrderBy(sortingColumnPropertyExpression);
		}
		else reviewsQuery = reviewsQuery.OrderByDescending(sortingColumnPropertyExpression);

		var reviewsDtos = _mapper.Map<List<ReviewDto>>(reviewsQuery.ToList());
		return PagedList<ReviewDto>.Create(reviewsDtos, request.Page);
	}

	private Expression<Func<Review, object>> GetSortingColumnProperty(GetBookReviewsQuery request) =>
		request.SortColumn?.ToLower() switch
		{
			"date" => review => review.CreationDate,
			_ => review => review.Rating
		};
}
