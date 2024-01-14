﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Reviews.Queries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Reviews.Queries.GetAllReviews;

public class GetAllReviewsQueryHandler : IRequestHandler<GetAllReviewsQuery, PagedList<ReviewDto>>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetAllReviewsQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<PagedList<ReviewDto>> Handle(GetAllReviewsQuery request, CancellationToken cancellationToken)
	{
		IQueryable<Review> reviewsQuery = _dbContext.Reviews;

		reviewsQuery = reviewsQuery.OrderByDescending(review => review.CreationDate);

		var reviewDtos = _mapper.Map<List<ReviewDto>>(await reviewsQuery.ToListAsync(cancellationToken));

		return PagedList<ReviewDto>.Create(reviewDtos, request.Page);
	}
}