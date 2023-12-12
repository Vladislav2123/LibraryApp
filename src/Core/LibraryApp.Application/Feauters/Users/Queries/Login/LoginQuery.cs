using MediatR;

namespace LibraryApp.Application.Feauters.Users.Queries.Login
{
	public record LoginQuery(
		string Email,
		string Password) 
		: IRequest<LoginResponse>;
}
