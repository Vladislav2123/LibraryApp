using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Pagination;
using LibraryApp.Application.Features.Authors.Queries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Authors.Queries.GetAllAuthors;
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
		IQueryable<Author> authorsQuery = _dbContext.Authors
			.AsNoTracking()
			.Select(author => new Author
			{
				Id = author.Id,
				Name = author.Name
			});

		if (string.IsNullOrWhiteSpace(request.SearchTerms) == false)
		{
			authorsQuery = authorsQuery.Where(author => author.Name.Contains(request.SearchTerms));
		}

		var totalAmount = authorsQuery.Count();
		var authors = authorsQuery
			.Skip((request.Page.number - 1) * request.Page.size)
			.Take(request.Page.size)
			.ToList();
		var mappedAuthors = _mapper.Map<List<AuthorLookupDto>>(authors);
		
		return new PagedList<AuthorLookupDto>(mappedAuthors, totalAmount, request.Page);
	}
}
