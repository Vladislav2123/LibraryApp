using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;

namespace LibraryApp.Application.Interfaces
{
	public interface ILibraryDbContext
	{
		DbSet<User> Users { get; set; }
		DbSet<Author> Authors { get; set; }
		DbSet<Book> Books { get; set; }
		DbSet<Review> Reviews { get; set; }

		void SaveChangesAsync(CancellationToken cancellationToken);
	}
}
