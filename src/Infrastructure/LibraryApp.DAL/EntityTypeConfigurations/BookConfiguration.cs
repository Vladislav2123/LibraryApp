using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.DAL.EntityTypeConfigurations
{
	public class BookConfiguration : IEntityTypeConfiguration<Book>
	{
		public void Configure(EntityTypeBuilder<Book> builder)
		{
			builder.Property(b => b.Name).HasMaxLength(256);
		}
	}
}
