using AutoMapper;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Common.Exceptions;
using LibraryApp.Application.Feauters.Users.Queries.Dto;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUserDetails
{
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
            User user = await _dbContext.Users
                .FirstOrDefaultAsync(user => user.Id == request.Id, cancellationToken);

            if (user == null) throw new EntityNotFoundException(nameof(User), request.Id);

            return _mapper.Map<UserDetailsDto>(user);
        }
    }
}
