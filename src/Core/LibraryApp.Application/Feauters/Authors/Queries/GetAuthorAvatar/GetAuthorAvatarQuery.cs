using LibraryApp.Domain.Models;
using MediatR;

namespace LibraryApp.Application.Feauters.Authors.Queries.GetAuthorAvatar
{
	public record GetAuthorAvatarQuery(Guid Id) : IRequest<FileVm>;
}
