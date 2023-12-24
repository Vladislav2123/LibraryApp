using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using LibraryApp.Domain.Exceptions;

namespace LibraryApp.Application.Feauters.Authors.Commands.Create;

public class CreateAuthorCommandHandler : IRequestHandler<CreateAuthorCommand, Guid>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly HttpContext? _httpContext;

	public CreateAuthorCommandHandler(ILibraryDbContext dbContext, IHttpContextAccessor httpContextAccessor)
	{
		_dbContext = dbContext;
		_httpContext = httpContextAccessor.HttpContext;
	}

	public async Task<Guid> Handle(CreateAuthorCommand command, CancellationToken cancellationToken)
	{
		if (await _dbContext.Authors
			.AnyAsync(author => 
				author.Name == command.Name &&
				author.BirthDate == command.BirthDate, cancellationToken))
		{
			throw new EntityAlreadyExistException(nameof(Author));
		}

		Guid userId = Guid.Parse(_httpContext.User.FindFirstValue(ClaimTypes.Actor));

		Author author = new Author()
		{
			Id = Guid.NewGuid(),
			Name = command.Name,
			BirthDate = command.BirthDate,
			CreationDate = DateTime.Now,
			CreatedUserId = userId
		};

		await _dbContext.Authors.AddAsync(author, cancellationToken);
		await _dbContext.SaveChangesAsync(cancellationToken);

		return author.Id;
	}
}
