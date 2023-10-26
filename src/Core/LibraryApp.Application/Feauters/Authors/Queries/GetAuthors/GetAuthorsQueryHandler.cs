using AutoMapper;
using LibraryApp.Application.Common.Helpers.Pagination;
using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthors
{
    public class GetAuthorsQueryHandler : IRequestHandler<GetAuthorsQuery, PagedList<AuthorLookupDto>>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

		public GetAuthorsQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
			_dbContext = dbContext;
			_mapper = mapper;
        }

        public async Task<PagedList<AuthorLookupDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
		{
			IQueryable<Author> authorsQuery = _dbContext.Authors;

			if(string.IsNullOrWhiteSpace(request.SearchTerms) == false)
			{
				authorsQuery = authorsQuery.Where(author => author.Name.Contains(request.SearchTerms));
			}

			var authorsLookups = _mapper.Map<List<AuthorLookupDto>>(await authorsQuery.ToListAsync(cancellationToken));
			return PagedList<AuthorLookupDto>.Create(authorsLookups, request.Page);
		}
	}
}
