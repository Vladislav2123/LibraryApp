using MediatR;
using Microsoft.EntityFrameworkCore;
using FileNotFoundException = LibraryApp.Domain.Exceptions.FileNotFoundException;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Authors.Commands.DeleteAuthorAvatar;

public class DeleteAuthorAvatarCommandHandler : IRequestHandler<DeleteAuthorAvatarCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

	public DeleteAuthorAvatarCommandHandler(
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

	public async Task<Unit> Handle(DeleteAuthorAvatarCommand command, CancellationToken cancellationToken)
	{
		var cacheKey = $"{_cacheKeys.Author}{command.AuthorId}";

		var author = await _cache
			.GetAsync(
				cacheKey,
				AuthorQuery,
				cancellationToken
			);

		if (string.IsNullOrEmpty(author.AvatarPath) ||
			Path.Exists(author.AvatarPath) == false)
			throw new FileNotFoundException("Author avatar");

		_fileWrapper.DeleteFile(author.AvatarPath);
		author.AvatarPath = string.Empty;

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
