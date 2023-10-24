using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Common.Exceptions;

namespace LibraryApp.Application.Feauters.Users.Commands.Delete
{
	public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
	{
		public readonly ILibraryDbContext _dbContext;

        public DeleteUserCommandHandler(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Unit> Handle(DeleteUserCommand command, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == command.Id);

            if (user == null) throw new EntityNotFoundException(nameof(User), command.Id);

            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
