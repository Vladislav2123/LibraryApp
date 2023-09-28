using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.DAL.EntityTypeConfigurations
{
	public class ReviewConfiguration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder.Property(r => r.Rating).HasMaxLength(5);
			builder.Property(r => r.Title).HasMaxLength(256);
		}
	}
}
