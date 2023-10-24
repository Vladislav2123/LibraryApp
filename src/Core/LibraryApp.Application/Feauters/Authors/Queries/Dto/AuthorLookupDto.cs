

using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Authors.Queries.Dto
{
	public class AuthorLookupDto : IMapWith<Author>
	{
		Guid Id { get; set; }
		string Name { get; set; }
	}
}
