using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Users.Commands.Update;

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
			user.Email == command.Email &&
			user.BirthDate == command.BirthDate)
		{
			throw new EntityHasNoChangesException(nameof(User), command.UserId);
		}

		if (await _dbContext.Users
			.AnyAsync(user =>
				user.Id != command.UserId &&
				user.Email == command.Email, cancellationToken))
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
