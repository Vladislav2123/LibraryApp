using AutoMapper;
using LibraryApp.Application.Mapping;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Features.Authors.Queries.Dto;

public class AuthorLookupDto : IMappping
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public string AvatarUrl { get; set; }

	public void CreateMap(Profile profile)
	{
		profile.CreateMap<Author, AuthorLookupDto>();
	}
}
