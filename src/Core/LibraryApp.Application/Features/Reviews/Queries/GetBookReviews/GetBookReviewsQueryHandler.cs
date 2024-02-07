using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Reviews.Queries.GetBookReviews;

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
		// Query
		var book = await _dbContext.Books
			.Include(book => book.Reviews)
			.FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

		if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

		IQueryable<Review> reviewsQuery = book.Reviews.AsQueryable();

		// Sorting
		var sortingColumnPropertyExpression = GetSortingColumnProperty(request);
		if (request.SortOrder?.ToLower() == "asc") reviewsQuery = reviewsQuery.OrderBy(sortingColumnPropertyExpression);
		else reviewsQuery = reviewsQuery.OrderByDescending(sortingColumnPropertyExpression);

		// Response
		var totalAmount = reviewsQuery.Count();
		var reviews = reviewsQuery
			.Skip((request.Page.number - 1) * request.Page.size)
			.Take(request.Page.size)
			.ToList();
		var mappedReviews = _mapper.Map<List<ReviewDto>>(reviews);

		return new PagedList<ReviewDto>(mappedReviews, totalAmount, request.Page);
	}

	private Expression<Func<Review, object>> GetSortingColumnProperty(GetBookReviewsQuery request) =>
		request.SortColumn?.ToLower() switch
		{
			"date" => review => review.CreationDate,
			_ => review => review.Rating
		};
}
