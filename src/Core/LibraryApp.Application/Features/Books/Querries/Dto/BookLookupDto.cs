using AutoMapper;
using LibraryApp.Application.Mapping;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Features.Books.Querries.Dto;

public class BookLookupDto : IMappping
{
	public Guid Id { get; set; }
	public Guid AuthorId { get; set; }
	public string Name { get; set; }
	public double Rating { get; set; }
	public int Year { get; set; }
	public int ReadersCount { get; set; }
	public string CoverUrl { get; set; }

	public void CreateMap(Profile profile)
	{
		profile.CreateMap<Book, BookLookupDto>()
			.ForMember(dest => dest.ReadersCount,
				opt => opt.MapFrom(src => src.Readers.Count));
	}
}
