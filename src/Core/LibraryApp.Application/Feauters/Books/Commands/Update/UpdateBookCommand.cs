﻿using MediatR;

namespace LibraryApp.Application.Feauters.Books.Commands.Update
{
	public record UpdateBookCommand(
		Guid Id,
		Guid AuthorId,
		string Name,
		string Description,
		int Year,
		string Text) : IRequest<Unit>;
}