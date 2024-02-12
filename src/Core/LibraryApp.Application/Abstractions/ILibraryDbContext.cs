using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using LibraryApp.Domain.Entities;
using System.Diagnostics.CodeAnalysis;

namespace LibraryApp.Application.Abstractions;

public interface ILibraryDbContext
{
	DatabaseFacade Database { get; }
	DbSet<User> Users { get; set; }
	DbSet<Author> Authors { get; set; }
	DbSet<Book> Books { get; set; }
	DbSet<Review> Reviews { get; set; }

	DbSet<TEntity> Set<
		[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors |
		DynamicallyAccessedMemberTypes.NonPublicConstructors |
		DynamicallyAccessedMemberTypes.PublicFields |
		DynamicallyAccessedMemberTypes.NonPublicFields |
		DynamicallyAccessedMemberTypes.PublicProperties |
		DynamicallyAccessedMemberTypes.NonPublicProperties |
		DynamicallyAccessedMemberTypes.Interfaces)]
	TEntity>() where TEntity : class;

	Microsoft.EntityFrameworkCore.ChangeTracking.EntityEntry<TEntity> Entry<TEntity>(TEntity entity)
		where TEntity : class;

	/// <summary>
	/// Returns the same object that tracking by ChangeTracker.
	/// </summary>
	object FindTrackingObject(object entity);
	Task<int> SaveChangesAsync(CancellationToken cancellationToken);
	int SaveChanges();
}
