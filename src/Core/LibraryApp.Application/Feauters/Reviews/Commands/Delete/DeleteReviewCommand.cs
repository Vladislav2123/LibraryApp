using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Delete;

public record DeleteReviewCommand(Guid ReviewId) 
	: IRequest<Unit>;
