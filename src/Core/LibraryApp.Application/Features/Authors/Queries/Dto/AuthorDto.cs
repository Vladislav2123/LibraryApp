using AutoMapper;
using LibraryApp.Application.Mapping;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Authors.Queries.Dto;

public class AuthorDto : IMappping
{
	public Guid CreatedUserId { get; set; }
	public string Name { get; set; }
	public DateOnly? BirthDate { get; set; }
	public string AvatarUrl { get; set; }

	public void CreateMap(Profile profile)
	{
		profile.CreateMap<Author, AuthorDto>();
	}
}
