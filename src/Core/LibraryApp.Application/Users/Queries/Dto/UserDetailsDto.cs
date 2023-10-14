using LibraryApp.Application.Common.Mappings;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Users.Queries.Dto
{
	public class UserDetailsDto : IMapWith<User>
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public string Login { get; set; }
		public string Email { get; set; }
		public DateTime BirthDate { get; set; }
	}
}
