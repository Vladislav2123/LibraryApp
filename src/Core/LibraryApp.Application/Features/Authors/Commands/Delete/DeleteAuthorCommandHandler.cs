using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Authors.Commands.Delete;

public class DeleteAuthorCommandHandler : IRequestHandler<DeleteAuthorCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public DeleteAuthorCommandHandler(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper,
		ICacheService cache,
		ICacheKeys cacheKeys)
	{
		_dbContext = dbContext;
		_fileWrapper = fileWrapper;
		_cache = cache;
		_cacheKeys = cacheKeys;
	}

	public async Task<Unit> Handle(DeleteAuthorCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Author}{command.AuthorId}";

		var author = await _cache
			.GetAsync(
				cacheKey, 
				AuthorQuery, 
				cancellationToken
			);

		_fileWrapper.DeleteFile(author.AvatarPath);

		_dbContext.Authors.Remove(author);
		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.RemoveAsync(cacheKey, cancellationToken);

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
