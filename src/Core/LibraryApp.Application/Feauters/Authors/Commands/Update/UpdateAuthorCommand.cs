using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.Update;

public record UpdateAuthorCommand(
	Guid AuthorId,
	string Name,
	DateOnly? BirthDate) : IRequest<Unit>;
