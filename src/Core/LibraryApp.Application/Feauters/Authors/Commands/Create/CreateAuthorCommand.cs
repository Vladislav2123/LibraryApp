using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.Create
{
	public record CreateAuthorCommand(
		string Name,
		DateOnly? BirthDate) : IRequest<Guid>;
}
