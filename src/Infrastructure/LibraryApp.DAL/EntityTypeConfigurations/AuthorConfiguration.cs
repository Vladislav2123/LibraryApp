using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.DAL.EntityTypeConfigurations
{
	public class AuthorConfiguration : IEntityTypeConfiguration<Author>
	{
		public void Configure(EntityTypeBuilder<Author> builder)
		{
			builder.HasKey(author => author.Id);

			builder.Property(author => author.Name)
				.HasMaxLength(50)
				.IsRequired();

			builder.Property(author => author.BirthDate)
				.HasColumnType("date");
		}
	}
}
