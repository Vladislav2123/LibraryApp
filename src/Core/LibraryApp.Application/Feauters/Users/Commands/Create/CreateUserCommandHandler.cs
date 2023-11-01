using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Users.Commands.Create
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
            if(await _dbContext.Users
                .AnyAsync(user => user.Email == command.Email, cancellationToken))
            {
                throw new UserEmailAlreadyUsingException(command.Email);
            }

            User newUser = new User()
            {
                Id = Guid.NewGuid(),
                Name = command.Name,
                Email = command.Email,
                Password = command.Password,
                BirthDate = command.BirthDate,
                CreationDate = DateTime.Now,
                Role = UserRole.Default
            };

            await _dbContext.Users.AddAsync(newUser, cancellationToken);
            await _dbContext.SaveChangesAsync(cancellationToken);

            return newUser.Id;
        }
    }
}
