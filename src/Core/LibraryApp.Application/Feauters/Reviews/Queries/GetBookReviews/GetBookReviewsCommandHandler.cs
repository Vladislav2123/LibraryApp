using AutoMapper;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Common.Helpers;
using LibraryApp.Application.Feauters.Reviews.Queries.Dto;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryApp.Application.Feauters.Reviews.Queries.GetBookReviews
{
    public class GetBookReviewsCommandHandler : IRequestHandler<GetBookReviewsCommand, PagedList<ReviewDto>>
    {
        private readonly ILibraryDbContext _dbContext;
        private readonly IMapper _mapper;

        public GetBookReviewsCommandHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<PagedList<ReviewDto>> Handle(GetBookReviewsCommand request, CancellationToken cancellationToken)
        {
            var book = await _dbContext.Books
                .Include(book => book.Reviews)
                .FirstOrDefaultAsync(book => book.Id == request.BookId, cancellationToken);

            if (book == null) throw new EntityNotFoundException(nameof(Book), request.BookId);

            IQueryable<Review>reviewsQuery = book.Reviews.AsQueryable();

            var sortingColumnPropertyExpression = GetSortingColumnProperty(request);
            if (request.SortOrder?.ToLower() == "asc")
            {
                reviewsQuery = reviewsQuery.OrderBy(sortingColumnPropertyExpression);
            }
            else reviewsQuery = reviewsQuery.OrderByDescending(sortingColumnPropertyExpression);

            var reviewsDtosQuery = _mapper.Map<IQueryable<ReviewDto>>(reviewsQuery);
            return await PagedList<ReviewDto>.CreateAsync(reviewsDtosQuery, request.Page, request.PageSize, cancellationToken);
        }

        private Expression<Func<Review, object>> GetSortingColumnProperty(GetBookReviewsCommand request)
        {
            return request.SortColumn?.ToLower() switch
            {
                "date" => review => review.Date,
                _ => review => review.Rating
            };
        }
    }
}
