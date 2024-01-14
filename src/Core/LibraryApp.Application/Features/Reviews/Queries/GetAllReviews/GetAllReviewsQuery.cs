using LibraryApp.Application.Pagination;
using MediatR;
using LibraryApp.Application.Features.Reviews.Queries.Dto;

namespace LibraryApp.Application.Features.Reviews.Queries.GetAllReviews;

public record GetAllReviewsQuery(Page Page)
	: IRequest<PagedList<ReviewDto>>;
