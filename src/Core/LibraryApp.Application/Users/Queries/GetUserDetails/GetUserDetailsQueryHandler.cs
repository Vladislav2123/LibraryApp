using AutoMapper;
using LibraryApp.Application.Interfaces;
using LibraryApp.Application.Users.Queries.Dto;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Common.Exceptions;

namespace LibraryApp.Application.Users.Queries.GetUser
{
	public class GetUserDetailsQueryHandler : IRequestHandler<GetUserDetailsQuery, UserDetailsDto>
	{
		private readonly ILibraryDbContext _dbContext;
		private readonly IMapper _mapper;

        public GetUserDetailsQueryHandler(ILibraryDbContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<UserDetailsDto> Handle(GetUserDetailsQuery query, CancellationToken cancellationToken)
        {
            User user = await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == query.Id, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(User), query.Id);

            return _mapper.Map<UserDetailsDto>(user);
        }
    }
}
