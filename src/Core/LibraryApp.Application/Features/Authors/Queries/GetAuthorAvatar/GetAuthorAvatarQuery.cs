using LibraryApp.Domain.Models;
using MediatR;

namespace LibraryApp.Application.Features.Authors.Queries.GetAuthorAvatar;

public record GetAuthorAvatarQuery(Guid AuthorId) 
	: IRequest<FileVm>;
