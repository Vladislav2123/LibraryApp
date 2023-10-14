using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Books.Querries.Dto
{
	public class BookLookupDto : IMapWith<Book>
	{
		public Guid Id { get; set; }
		public Guid AuthorId { get; set; }
		public string Name { get; set; }
		public string Year { get; set; }
		public ICollection<Guid> ReadUsers { get; set; }

		public void CreateMap(Profile profile)
		{
			profile.CreateMap<Book, BookLookupDto>()
				.ForMember(dest => dest.ReadUsers,
					opt => opt.MapFrom(src => src.ReadUsers.Select(user => user.Id)));
		}
	}
}
