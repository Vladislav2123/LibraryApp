using MediatR;

namespace LibraryApp.Application.Features.Authors.Commands.Delete;

public record DeleteAuthorCommand(Guid AuthorId) 
	: IRequest<Unit>;
