using MediatR;
using LibraryApp.Application.Features.Users.Queries.Dto;


namespace LibraryApp.Application.Feauters.Users.Queries.GetUserDetails;

public record GetUserQuery(Guid UserId) 
	: IRequest<UserDetailsDto>;
