using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.Create
{
	public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Guid>
	{
		private readonly ILibraryDbContext _dbContext;

		public CreateAuthorCommandHandler(ILibraryDbContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Guid> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
		{
			if (_dbContext.Users.Any(user =>
				user.Id == command.UserId) == false)
			{
				throw new EntityNotFoundException(nameof(User), command.UserId);
			}

			if (_dbContext.Authors.Any(author =>
				author.Name == command.Name &&
				author.BirthDate == command.BirthDate))
			{
				throw new EntityAlreadyExistException(nameof(Author));
			}

			Author author = new Author()
			{
				Id = Guid.NewGuid(),
				Name = command.Name,
				BirthDate = command.BirthDate,
				CreationDate = DateTime.Now,
				CreatedUserId = command.UserId
			};

			await _dbContext.Authors.AddAsync(author, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return author.Id;
		}
	}
}
