using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.DAL.EntityTypeConfigurations
{
	public class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.HasKey(book => book.Id);

			builder.Property(book => book.Name)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(book => book.Description)
				.HasMaxLength(1000);

			builder.Property(book => book.Year)
				.HasMaxLength(4);
		}
	}
}
