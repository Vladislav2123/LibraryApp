using MediatR;

namespace LibraryApp.Application.Features.Authors.Commands.Create;

public record CreateAuthorCommand(
	string Name,
	DateOnly? BirthDate) 
	: IRequest<Guid>;
