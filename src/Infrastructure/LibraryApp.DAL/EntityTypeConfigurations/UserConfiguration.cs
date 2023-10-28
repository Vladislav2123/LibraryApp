using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.DAL.EntityTypeConfigurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.HasKey(user => user.Id);

			builder.Property(user => user.Name)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(user => user.Email)
				.HasMaxLength(100)
				.IsRequired();
			builder.HasIndex(user => user.Email)
				.IsUnique();

			builder.Property(user => user.Password)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(user => user.BirthDate)
				.IsRequired()
				.HasColumnType("date");

			builder.Property(user => user.CreationDate)
				.IsRequired();

			builder.Property(user => user.Role)
				.IsRequired()
				.HasConversion(r => r.ToString(), sr => Enum.Parse<UserRole>(sr));

			builder.HasMany(user => user.ReadBooks)
				.WithMany(book => book.Readers);

			builder.HasMany(user => user.CreatedBooks)
				.WithOne(book => book.CreatedUser)
				.HasForeignKey(book => book.CreatedUserId)
				.OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(user => user.CreatedAuthors)
				.WithOne(author => author.CreatedUser)
				.HasForeignKey(author => author.CreatedUserId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
