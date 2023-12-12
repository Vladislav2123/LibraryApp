using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Users.Queries.Dto
{
	public class UserDetailsDto : IMappping
	{
		public string Name { get; set; }
		public string Email { get; set; }
		public DateOnly BirthDate { get; set; }
		public string Role { get; set; }
		public string AvatarUrl { get; set; }

		public void CreateMap(Profile profile)
		{
			profile.CreateMap<User, UserDetailsDto>()
				.ForMember(dest => dest.Role,
					opt => opt.MapFrom(src => src.Role.ToString()));
		}
	}
}
