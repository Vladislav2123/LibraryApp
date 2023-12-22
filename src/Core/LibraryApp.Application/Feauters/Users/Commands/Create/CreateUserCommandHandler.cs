using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Users.Commands.Create;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IPasswordProvider _passwordHasher;

	public CreateUserCommandHandler(ILibraryDbContext dbContext, IPasswordProvider passwordHasher)
	{
		_dbContext = dbContext;
		_passwordHasher = passwordHasher;
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

		User newUser = new User()
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

		await _dbContext.Users.AddAsync(newUser, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return newUser.Id;
	}
}
