using AutoMapper;
using LibraryApp.Application.Mapping;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Users.Queries.Dto;

public class UserLookupDto : IMappping
{
	public Guid Id { get; set; }
	public string Name { get; set; }
	public DateOnly BirthDate { get; set; }
	public int ReadBooksCount { get; set; }
	public string Role { get; set; }
	public string AvatarUrl { get; set; }

	public void CreateMap(Profile profile)
	{
		profile.CreateMap<User, UserLookupDto>()
			.ForMember(dest => dest.ReadBooksCount,
				opt => opt.MapFrom(src => src.ReadBooks.Count))
			.ForMember(dest => dest.Role,
				opt => opt.MapFrom(user => user.Role.ToString()));
	}
}
