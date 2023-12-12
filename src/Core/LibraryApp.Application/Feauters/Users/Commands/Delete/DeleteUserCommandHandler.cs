using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Abstractions;

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
            User user = await _dbContext.Users
                .Include(user => user.CreatedAuthors)
                .FirstOrDefaultAsync(user => user.Id == command.UserId);

            if (user == null) throw new EntityNotFoundException(nameof(User), command.UserId);

            if(string.IsNullOrEmpty(user.AvatarPath) == false)
                File.Delete(user.AvatarPath);

            _dbContext.Authors.RemoveRange(user.CreatedAuthors);
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
