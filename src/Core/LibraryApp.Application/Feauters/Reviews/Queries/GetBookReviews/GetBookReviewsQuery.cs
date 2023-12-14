using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetBookReviews;

    public record GetBookReviewsQuery(
	Guid BookId,
	string? SortColumn,
	string? SortOrder,
	Page Page) 
	: IRequest<PagedList<ReviewDto>>;
