using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Authors.Commands.Create;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Guid>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly HttpContext? _httpContext;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

    public CreateAuthorCommandHandler(
        ILibraryDbContext dbContext,
        IHttpContextAccessor httpContextAccessor,
        ICacheService cache,
        ICacheKeys cacheKeys)
    {
        _dbContext = dbContext;
        _httpContext = httpContextAccessor.HttpContext;
        _cache = cache;
        _cacheKeys = cacheKeys;
    }

    public async Task<Guid> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
	{
		if (await _dbContext.Authors
			.AnyAsync(author => 
				author.Name == command.Name &&
				author.BirthDate == command.BirthDate, cancellationToken))
		{
			throw new EntityAlreadyExistException(nameof(Author));
		}

		Guid userId = Guid.Parse(_httpContext.User.FindFirstValue(ClaimTypes.Actor));

		Author author = new Author()
		{
			Id = Guid.NewGuid(),
			Name = command.Name,
			BirthDate = command.BirthDate,
			CreationDate = DateTime.Now,
			CreatedUserId = userId
		};

		await _dbContext.Authors.AddAsync(author, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.SetAsync($"{_cacheKeys.Author}{author.Id}", author, cancellationToken);

		return author.Id;
	}
}
