using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Enteties;
using LibraryApp.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace LibraryApp.Application.Feauters.Users.Commands.UpdateUserRole;
public class UpdateUserRoleCommandHandler : IRequestHandler<UpdateUserRoleCommand, Unit>
{
    private readonly ILibraryDbContext _dbContext;

	public UpdateUserRoleCommandHandler(ILibraryDbContext dbContext)
	{
		_dbContext = dbContext;
	}

	public async Task<Unit> Handle(UpdateUserRoleCommand command, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users
			.FirstOrDefaultAsync(user => user.Id == command.UserId, cancellationToken);

		if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

		user.Role = Enum.Parse<UserRole>(command.Role);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return Unit.Value;
	}
}
