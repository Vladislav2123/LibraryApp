using MediatR;

namespace LibraryApp.Application.Features.Users.Queries.Login;

public record LoginQuery(
	string Email,
	string Password) 
	: IRequest<LoginResponse>;
