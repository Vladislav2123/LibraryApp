using MediatR;
using LibraryApp.Application.Features.Authors.Queries.Dto;

namespace LibraryApp.Application.Features.Authors.Queries.GetAuthor;

public record GetAuthorQuery(Guid AuthorId) 
	: IRequest<AuthorDto>;
