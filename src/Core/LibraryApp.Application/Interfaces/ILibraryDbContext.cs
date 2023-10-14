using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using System.Reflection;

namespace LibraryApp.Application.Interfaces
{
	public interface ILibraryDbContext
	{
		DbSet<User> Users { get; set; }
		DbSet<Author> Authors { get; set; }
		DbSet<Book> Books { get; set; }
		DbSet<Review> Reviews { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	}
}
