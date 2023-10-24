using AutoMapper;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Authors.Queries.Dto;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthor
{
	public class GetAuthorQueryHandler : IRequestHandler<GetAuthorQuery, AuthorDto>
	{
		private readonly ILibraryDbContext _dbContext;

		private readonly IMapper _mapper;

		public GetAuthorQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
		{
			_dbContext = dbContext;
			_mapper = mapper;
		}

		public async Task<AuthorDto> Handle(GetAuthorQuery request, CancellationToken cancellationToken)
		{
			var author = await _dbContext.Authors.FirstOrDefaultAsync(author => author.Id == request.Id, cancellationToken);

			if (author == null) throw new EntityNotFoundException(nameof(Author), request.Id);

			return _mapper.Map<AuthorDto>(author);
		}
	}
}
