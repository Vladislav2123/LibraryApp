using AutoMapper;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.Create
{
	public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Guid>
	{
		private readonly ILibraryDbContext _dbContext;

		private readonly IMapper _mapper;

		public CreateAuthorCommandHandler(ILibraryDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<Guid> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
		{
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
