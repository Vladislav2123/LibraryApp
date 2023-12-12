﻿using LibraryApp.Domain.Models;
using MediatR;

namespace LibraryApp.Application.Feauters.Books.Querries.GetBookContent
{
	public record GetBookContentQuery(Guid BookId) 
		: IRequest<FileVm>;
}
