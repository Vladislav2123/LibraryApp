using LibraryApp.Application.Users.Queries.Dto;
using MediatR;


namespace LibraryApp.Application.Users.Queries.GetUser
{
	public record GetUserDetailsQuery(Guid Id) : IRequest<UserDetailsDto>;
}
