using MediatR;

namespace LibraryApp.Application.Feauters.Reviews.Commands.Create;

public record CreateReviewCommand(
	Guid BookId,
	byte Rating,
	string? Title,
	string? Comment)
	: IRequest<Guid>;
