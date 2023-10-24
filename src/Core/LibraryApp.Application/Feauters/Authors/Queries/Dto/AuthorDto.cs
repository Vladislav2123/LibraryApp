using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Authors.Queries.Dto
{
	public class AuthorDto : IMapWith<Author>
	{
		public string Name { get; set; }
		public DateOnly? BirthDate { get; set; }
	}
}
