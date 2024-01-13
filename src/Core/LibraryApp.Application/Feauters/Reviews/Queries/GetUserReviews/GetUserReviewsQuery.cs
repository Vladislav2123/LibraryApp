using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Pagination;
using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetUserReviews;

public record GetUserReviewsQuery(
	Guid UserId,
	Page Page)
	: IRequest<PagedList<ReviewDto>>;
