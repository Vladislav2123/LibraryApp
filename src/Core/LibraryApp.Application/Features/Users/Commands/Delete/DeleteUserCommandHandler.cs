using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Users.Commands.Delete;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IFileWrapper _fileWrapper;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

    public DeleteUserCommandHandler(ILibraryDbContext dbContext, IFileWrapper fileWrapper, ICacheService cacheService, ICacheKeys cacheKeys)
    {
        _dbContext = dbContext;
        _fileWrapper = fileWrapper;
        _cache = cacheService;
        _cacheKeys = cacheKeys;
    }

    public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
	{
		User user = await _dbContext.Users
			.Include(user => user.CreatedAuthors)
			.FirstOrDefaultAsync(user => user.Id == command.UserId);

		if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

		_fileWrapper.DeleteFile(user.AvatarPath);

		_dbContext.Authors.RemoveRange(user.CreatedAuthors);
		_dbContext.Users.Remove(user);
		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.RemoveAsync($"{_cacheKeys.User}{user.Id}", cancellationToken);

		return Unit.Value;
	}
}
