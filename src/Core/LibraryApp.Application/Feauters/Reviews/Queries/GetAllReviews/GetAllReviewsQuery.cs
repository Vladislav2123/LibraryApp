using LibraryApp.Application.Common.Helpers.Pagination;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetAllReviews
{
	public record GetAllReviewsQuery(Page Page) : IRequest<PagedList<ReviewDto>>;
}
