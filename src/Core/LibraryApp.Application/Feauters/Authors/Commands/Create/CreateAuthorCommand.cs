using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.Create
{
	public record CreateAuthorCommand(
		Guid UserId,
		string Name,
		DateOnly? BirthDate) : IRequest<Guid>;
}
