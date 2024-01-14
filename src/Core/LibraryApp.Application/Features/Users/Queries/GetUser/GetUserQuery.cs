using MediatR;
using LibraryApp.Application.Features.Users.Queries.Dto;


namespace LibraryApp.Application.Features.Users.Queries.GetUser;

public record GetUserQuery(Guid UserId) 
	: IRequest<UserDetailsDto>;
