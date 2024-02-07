using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Reviews.Queries.GetUserReviews;

public class GetUserReviewsQueryHandler : IRequestHandler<GetUserReviewsQuery, PagedList<ReviewDto>>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetUserReviewsQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PagedList<ReviewDto>> Handle(GetUserReviewsQuery request, CancellationToken cancellationToken)
	{
		var user = _dbContext.Users
			.Include(user => user.Reviews)
			.FirstOrDefault(user => user.Id == request.UserId);

		if (user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

		// Response
		var totalAmount = user.Reviews.Count;
		var reviews = user.Reviews
			.OrderByDescending(review => review.CreationDate)
			.Skip((request.Page.number - 1) * request.Page.size)
			.Take(request.Page.size)
			.ToList();
		var reviewsDtos = _mapper.Map<List<ReviewDto>>(reviews);

		return new PagedList<ReviewDto>(reviewsDtos, totalAmount, request.Page);
	}
}
