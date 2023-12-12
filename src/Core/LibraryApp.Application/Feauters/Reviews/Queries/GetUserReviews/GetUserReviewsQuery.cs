using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetUserReviews
{
    public record GetUserReviewsQuery(
		Guid UserId,
		Page Page) 
		: IRequest<PagedList<ReviewDto>>;
}
