using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Abstractions.Caching;

namespace LibraryApp.Application.Features.Users.Commands.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IPasswordProvider _passwordHasher;
	private readonly ICacheService _cache;
	private readonly ICacheKeys _cacheKeys;

    public CreateUserCommandHandler(
		ILibraryDbContext dbContext, 
		IPasswordProvider passwordHasher, 
		ICacheService cacheService, 
		ICacheKeys cacheKeys)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _cache = cacheService;
        _cacheKeys = cacheKeys;
    }

    public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
	{
		if (await _dbContext.Users
			.AnyAsync(user => user.Email == command.Email, cancellationToken))
		{
			throw new EmailAlreadyInUseException(command.Email);
		}

		string passwordSalt = BCrypt.Net.BCrypt.GenerateSalt();
		string passwordHash = _passwordHasher.HashPassword(command.Password, passwordSalt);

		User user = new User()
		{
			Id = Guid.NewGuid(),
			Name = command.Name,
			Email = command.Email,
			PasswordHash = passwordHash,
			PasswordSalt = passwordSalt,
			BirthDate = command.BirthDate,
			CreationDate = DateTime.Now,
			Role = UserRole.Default
		};

		await _dbContext.Users.AddAsync(user, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);
		await _cache.SetAsync($"{_cacheKeys.User}{user.Id}", user, cancellationToken);

		return user.Id;
	}
}
