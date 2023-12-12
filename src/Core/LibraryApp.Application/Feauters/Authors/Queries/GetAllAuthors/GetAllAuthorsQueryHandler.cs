using AutoMapper;
using LibraryApp.Application.Common.Pagination;
using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthors
{
    public class GetAllAuthorsQueryHandler : IRequestHandler<GetAllAuthorsQuery, PagedList<AuthorLookupDto>>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

		public GetAllAuthorsQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
			_dbContext = dbContext;
			_mapper = mapper;
        }

        public async Task<PagedList<AuthorLookupDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
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
