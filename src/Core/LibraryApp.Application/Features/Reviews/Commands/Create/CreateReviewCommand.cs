﻿using MediatR;

namespace LibraryApp.Application.Features.Reviews.Commands.Create;

public record CreateReviewCommand(
	Guid BookId,
	byte Rating,
	string? Title,
	string? Comment)
	: IRequest<Guid>;