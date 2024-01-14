using AutoMapper;
using LibraryApp.Application.Mapping;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Features.Reviews.Queries.Dto;

public class ReviewDto : IMappping
{
	public Guid Id { get; set; }
	public Guid UserId { get; set; }
	public DateTime CreationDate { get; set; }
	public int Rating { get; set; }
	public string? Title { get; set; }
	public string? Comment { get; set; }

	public void CreateMap(Profile profile)
	{
		profile.CreateMap<Review, ReviewDto>();
	}
}
