using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;

namespace LibraryApp.Application.Users.Commands.Create
{
	public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
	{
		private readonly ILibraryDbContext _dbContext;

        public CreateUserCommandHandler(ILibraryDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Guid> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Email = command.Email,
                Login = command.Login,
                Password = command.Password,
                BirthDate = command.BirthDate,
            };

            await _dbContext.Users.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newUser.Id;
        }
    }
}
