using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Interfaces;
using LibraryApp.Domain.Enteties;
using LibraryApp.DAL.EntityTypeConfigurations;
using LibraryApp.DAL.ValueConverters;

namespace LibraryApp.DAL
{
	public class LibraryDbContext : DbContext, ILibraryDbContext
	{
		public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) 
		{
			Database.EnsureCreated();
		}

		public DbSet<User> Users { get; set; }
		public DbSet<Author> Authors { get; set; }
		public DbSet<Book> Books { get; set; }
		public DbSet<Review> Reviews { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new UserConfiguration());
			builder.ApplyConfiguration(new AuthorConfiguration());
			builder.ApplyConfiguration(new BookConfiguration());
			builder.ApplyConfiguration(new ReviewConfiguration());
		}

		protected override void ConfigureConventions(ModelConfigurationBuilder builder)
		{
			base.ConfigureConventions(builder);

			builder.Properties<DateOnly>()
				.HaveConversion<DateOnlyConverter>()
				.HaveColumnType("date");

			builder.Properties<DateOnly?>()
				.HaveConversion<DateOnlyNullableConverter>()
				.HaveColumnType("date");
		}
	}
}
