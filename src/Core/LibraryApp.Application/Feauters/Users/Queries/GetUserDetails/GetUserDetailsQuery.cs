using MediatR;
using LibraryApp.Application.Feauters.Users.Queries.Dto;


namespace LibraryApp.Application.Feauters.Users.Queries.GetUserDetails
{
	public record GetUserDetailsQuery(Guid Id) : IRequest<UserDetailsDto>;
}
