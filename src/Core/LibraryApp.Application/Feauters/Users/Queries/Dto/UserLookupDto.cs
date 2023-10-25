using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Users.Queries.Dto
{
	public class UserLookupDto : IMappping
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public DateOnly BirthDate { get; set; }
		public int ReadBooksCount { get; set; }

		public void CreateMap(Profile profile)
		{
			profile.CreateMap<User, UserLookupDto>()
				.ForMember(dest => dest.ReadBooksCount,
					opt => opt.MapFrom(src => src.ReadedBooks.Count));
		}
	}
}
