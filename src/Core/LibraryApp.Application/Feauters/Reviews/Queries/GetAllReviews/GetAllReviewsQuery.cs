using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Pagination;
using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetAllReviews;

public record GetAllReviewsQuery(Page Page)
	: IRequest<PagedList<ReviewDto>>;
