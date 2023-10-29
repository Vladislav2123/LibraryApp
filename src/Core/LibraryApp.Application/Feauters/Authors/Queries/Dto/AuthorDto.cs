using AutoMapper;
using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Authors.Queries.Dto
{
	public class AuthorDto : IMappping
	{
		public Guid CreatedUserId { get; set; }
		public string Name { get; set; }
		public DateOnly? BirthDate { get; set; }

		public void CreateMap(Profile profile)
		{
			profile.CreateMap<Author, AuthorDto>();
		}
	}
}
