using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using LibraryApp.Domain.Entities;
using AutoMapper;
using MediatR;

namespace LibraryApp.Application.Features.Reviews.Queries.GetAllReviews;

public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, PagedList<ReviewDto>>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllReviewsQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PagedList<ReviewDto>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
	{
		IQueryable<Review> reviewsQuery = _dbContext.Reviews
			.Select(review => new Review
			{
				Id = review.Id,
				UserId = review.UserId,
				CreationDate = review.CreationDate,
				Rating = review.Rating,
				Title = review.Title,
				Comment = review.Comment
			})
			.OrderByDescending(review => review.CreationDate);

		var totalAmount = reviewsQuery.Count();
		var reviews = reviewsQuery
			.Skip((request.Page.number - 1) * request.Page.size)
			.Take(request.Page.size)
			.ToList();
		var mappedReviews = _mapper.Map<List<ReviewDto>>(reviews);

		return new PagedList<ReviewDto>(mappedReviews, totalAmount, request.Page);
	}
}
