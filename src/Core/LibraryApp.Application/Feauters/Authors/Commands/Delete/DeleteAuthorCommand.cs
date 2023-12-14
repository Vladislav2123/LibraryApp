using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Commands.Delete;

public record DeleteAuthorCommand(Guid AuthorId) 
	: IRequest<Unit>;
