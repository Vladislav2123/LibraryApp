using LibraryApp.Application.Pagination;
using MediatR;
using LibraryApp.Application.Features.Reviews.Queries.Dto;

namespace LibraryApp.Application.Features.Reviews.Queries.GetBookReviews;

public record GetBookReviewsQuery(
	Guid BookId,
	string? SortColumn,
	string? SortOrder,
	Page Page)
	: IRequest<PagedList<ReviewDto>>;
