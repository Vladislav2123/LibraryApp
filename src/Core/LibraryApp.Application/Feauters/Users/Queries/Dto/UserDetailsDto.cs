using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Feauters.Users.Queries.Dto
{
	public class UserDetailsDto : IMapWith<User>
	{
		public string Name { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public DateOnly BirthDate { get; set; }
	}
}
