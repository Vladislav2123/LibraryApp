﻿using LibraryApp.Domain.Models;
using MediatR;

namespace LibraryApp.Application.Feauters.Users.Queries.GetUserAvatar
{
	public record GetUserAvatarQuery(Guid UserId) 
		: IRequest<FileVm>;
}
