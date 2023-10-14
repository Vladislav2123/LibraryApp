using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Books.Querries.Dto
{
	public class ReadUserDto : IMapWith<User>
	{
		public Guid Id { get; set; }
	}
}
