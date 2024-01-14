using LibraryApp.Application.Pagination;
using MediatR;
using LibraryApp.Application.Features.Reviews.Queries.Dto;

namespace LibraryApp.Application.Features.Reviews.Queries.GetUserReviews;

public record GetUserReviewsQuery(
	Guid UserId,
	Page Page)
	: IRequest<PagedList<ReviewDto>>;
