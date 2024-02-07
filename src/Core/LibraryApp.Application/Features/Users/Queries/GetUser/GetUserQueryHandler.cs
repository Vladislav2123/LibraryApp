using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Exceptions;
using LibraryApp.Application.Features.Users.Queries.Dto;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Users.Queries.GetUser;

public class GetUserQueryHandler : IRequestHandler<GetUserQuery, UserDetailsDto>
{
	private readonly ILibraryDbContext _dbContext;
	private readonly IMapper _mapper;

	public GetUserQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
	{
		_dbContext = dbContext;
		_mapper = mapper;
	}

	public async Task<UserDetailsDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
	{
		var user = await _dbContext.Users
			.AsNoTracking()
			.Select(user => new User
			{
				Id = user.Id,
				Name = user.Name,
				Email = user.Email,
				BirthDate = user.BirthDate,
				Role = user.Role
			})
			.FirstOrDefaultAsync(user => user.Id == request.UserId, cancellationToken);

		if (user == null) throw new EntityNotFoundException(nameof(User), request.UserId);

		return _mapper.Map<UserDetailsDto>(user);
	}
}
