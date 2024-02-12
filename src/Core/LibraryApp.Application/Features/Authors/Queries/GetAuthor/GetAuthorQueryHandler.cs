using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Authors.Queries.Dto;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Authors.Queries.GetAuthor;

public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, AuthorDto>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;
	private readonly ICacheService _cacheService;
	private readonly ICacheKeys _cacheKeys;

    public GetAuthorQueryHandler(
        ILibraryDbContext dbContext,
        IMapper mapper,
        ICacheService cacheService,
        ICacheKeys cacheKeys)
    {
        _dbContext = dbContext;
        _mapper = mapper;
        _cacheService = cacheService;
        _cacheKeys = cacheKeys;
    }

    public async Task<AuthorDto> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
	{
		var author = await _cacheService
			.GetAndSetAsync(
				$"{_cacheKeys.Author}{request.AuthorId}",
				AuthorFactory,
				cancellationToken);

		return _mapper.Map<AuthorDto>(author);

		async Task<Author> AuthorFactory()
		{
			var author = await _dbContext.Authors
				.AsNoTracking()
				.Select(author => new Author
				{
					Id = author.Id,
					CreatedUserId = author.CreatedUserId,
					Name = author.Name,
					BirthDate = author.BirthDate
				})
			.FirstOrDefaultAsync(author => author.Id == request.AuthorId, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), request.AuthorId);

			return author;
		}
	}
}
