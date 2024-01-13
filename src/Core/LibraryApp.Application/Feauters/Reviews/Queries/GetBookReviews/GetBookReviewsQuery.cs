using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Pagination;
using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetBookReviews;

public record GetBookReviewsQuery(
	Guid BookId,
	string? SortColumn,
	string? SortOrder,
	Page Page)
	: IRequest<PagedList<ReviewDto>>;
