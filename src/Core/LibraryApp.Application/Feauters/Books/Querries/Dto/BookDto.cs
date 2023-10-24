using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Books.Querries.Dto
{
    public class BookDto : IMapWith<Book>
	{
		public Guid AuthorId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int Year { get; set; }
		public string Text { get; set; }
		public ICollection<Guid> ReadUsers { get; set; }

		public void CreateMap(Profile profile)
		{
			profile.CreateMap<Book, BookDto>()
				.ForMember(dest => dest.ReadUsers,
					opt => opt.MapFrom(src => src.ReadUsers.Select(user => user.Id)));
		}
	}
}
