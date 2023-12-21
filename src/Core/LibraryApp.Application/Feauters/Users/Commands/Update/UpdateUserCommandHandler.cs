using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.Application.Feauters.Users.Commands.Update;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
{
	private readonly ILibraryDbContext _dbContext;

	public UpdateUserCommandHandler(ILibraryDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
	{
		User user = await _dbContext.Users
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

		if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

		if (user.Name == command.Name &&
			user.Email == command.Name &&
			user.BirthDate == command.BirthDate)
		{
			throw new EntityHasNoChangesException(nameof(User), command.UserId);
		}

		if (await _dbContext.Users
			.AnyAsync(u =>
				u.Id != user.Id &&
				u.Email == command.Email, cancellationToken))
		{
			throw new EmailAlreadyInUseException(command.Email);
		}

		user.Name = command.Name;
		user.Email = command.Email;
		user.BirthDate = command.BirthDate;

		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
