using AutoMapper;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.Create
{
	public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Guid>
	{
		private readonly ILibraryDbContext _dbContext;

		public CreateAuthorCommandHandler(ILibraryDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
		}

		public async Task<Guid> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
		{
			if(_dbContext.Authors.Any(author => 
			author.Name == request.Name && 
			author.BirthDate == request.BirthDate))
			{
				throw new EntityAlreadyExistException(nameof(Author));
			}

			Author author = new Author()
			{
				Id = Guid.NewGuid(),
				Name = request.Name,
				BirthDate = request.BirthDate
			};

			await _dbContext.Authors.AddAsync(author, cancellationToken);
			await _dbContext.SaveChangesAsync(cancellationToken);

			return author.Id;
		}
	}
}
