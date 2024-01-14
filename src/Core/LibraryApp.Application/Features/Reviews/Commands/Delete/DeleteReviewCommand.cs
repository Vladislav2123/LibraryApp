using MediatR;

namespace LibraryApp.Application.Features.Reviews.Commands.Delete;

public record DeleteReviewCommand(Guid ReviewId) 
	: IRequest<Unit>;
