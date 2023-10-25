using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetBookReviews
{
	public record GetBookReviewsCommand(
		Guid BookId,
		string? SortColumn,
		string? SortOrder,
		int Page,
		int Limit) : IRequest<PagedList<ReviewDto>>;
}
