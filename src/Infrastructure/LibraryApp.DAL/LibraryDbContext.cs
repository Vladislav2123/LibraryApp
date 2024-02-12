using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using LibraryApp.DAL.EntityTypeConfigurations;
using LibraryApp.DAL.ValueConverters;
using LibraryApp.Application.Abstractions;
using LibraryApp.Domain.Entities;

namespace LibraryApp.DAL;

public class LibraryDbContext : DbContext, ILibraryDbContext
{
	public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base(options) 
	{
		AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
	}

	public DbSet<User> Users { get; set; }
	public DbSet<Author> Authors { get; set; }
	public DbSet<Book> Books { get; set; }
	public DbSet<Review> Reviews { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

		optionsBuilder.UseSnakeCaseNamingConvention();
		optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
    }

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

    public object FindTrackingObject(object entity)
	{
		var entityType = Model.FindRuntimeEntityType(entity.GetType());
            var key = entityType.FindPrimaryKey();

            var keyProperties = key.Properties;

            var keyValues = new object[keyProperties.Count];
            for (int i = 0; i < keyValues.Length; i++)
            {
                keyValues[i] = keyProperties[i].GetGetter().GetClrValue(entity);
                Console.WriteLine($"Key Value {i}: {keyValues[i]}");
            }

            var stateManager = this.GetService<IStateManager>();
            return stateManager.TryGetEntry(key, keyValues)?.Entity;
	}
}
