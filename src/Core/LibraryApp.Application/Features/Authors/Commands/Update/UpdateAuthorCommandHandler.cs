using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Authors.Commands.Update;

public class UpdateAuthorCommandHandler : IRequestHandler<UpdateAuthorCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

    public UpdateAuthorCommandHandler(
		ILibraryDbContext dbContext, 
		ICacheService cache, 
		ICacheKeys cacheKeys)
    {
        _dbContext = dbContext;
        _cache = cache;
        _cacheKeys = cacheKeys;
    }

    public async Task<Unit> Handle(UpdateAuthorCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Author}{command.AuthorId}";

		var author = await _cache
			.GetAsync(
				cacheKey,
				AuthorQuery,
				cancellationToken
			);

		var sameAuthor = await _dbContext.Authors
			.FirstOrDefaultAsync(author =>
				author.Name == command.Name &&
				author.BirthDate == command.BirthDate, cancellationToken);

		if (sameAuthor != null)
		{
			if (sameAuthor.Id == author.Id)
				throw new EntityHasNoChangesException(nameof(Author), command.AuthorId);
			else throw new EntityAlreadyExistException(nameof(Author));
		}

		author.Name = command.Name;
		author.BirthDate = command.BirthDate;

		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.SetAsync(cacheKey, author, cancellationToken);

		return Unit.Value;

		async Task<Author> AuthorQuery()
		{
			var author = await _dbContext.Authors
				.FirstOrDefaultAsync(author => author.Id == command.AuthorId, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), command.AuthorId);

			return author;
		}
	}
}
