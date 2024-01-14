﻿using MediatR;

namespace LibraryApp.Application.Features.Reviews.Commands.Update;

public record UpdateReviewCommand(
	Guid ReviewId,
	byte Rating,
	string? Title,
	string? Comment) 
	: IRequest<Unit>;