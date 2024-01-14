using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LibraryApp.Domain.Entities;

namespace LibraryApp.Application.Abstractions;

public interface ILibraryDbContext
{
	DatabaseFacade Database { get; }
	DbSet<User> Users { get; set; }
	DbSet<Author> Authors { get; set; }
	DbSet<Book> Books { get; set; }
	DbSet<Review> Reviews { get; set; }

	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	int SaveChanges();
}
