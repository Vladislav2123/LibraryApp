using AutoMapper;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Common.Helpers.Pagination;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetUserReviews
{
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

			IQueryable<Review> reviewsQuery = user.Reviews.AsQueryable();

			reviewsQuery.OrderByDescending(user => user.Date);

			var reviewsDtos = _mapper.Map<List<ReviewDto>>(await reviewsQuery.ToListAsync(cancellationToken));
			return PagedList<ReviewDto>.Create(reviewsDtos, request.Page);
		}
	}
}
