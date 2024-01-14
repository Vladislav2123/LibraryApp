using AutoMapper;
using LibraryApp.Application.Mapping;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Books.Querries.Dto;

public class BookDto : IMappping
{
	public Guid Id { get; set; }
	public Guid CreatedUserId { get; set; }
	public Guid AuthorId { get; set; }
	public string Name { get; set; }
	public double Rating { get; set; }
	public string Description { get; set; }
	public int Year { get; set; }
	public string ContentUrl { get; set; }
	public string CoverUrl { get; set; }

	public void CreateMap(Profile profile)
	{
		profile.CreateMap<Book, BookDto>();
	}
}
