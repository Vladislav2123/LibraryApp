using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using LibraryApp.Domain.Entities;

namespace LibraryApp.DAL.EntityTypeConfigurations;

public class ReviewConfiguration : IEntityTypeConfiguration<Review>
{
	public void Configure(EntityTypeBuilder<Review> builder)
	{
		builder.HasKey(review => review.Id);

		builder.Property(review => review.UserId)
			.IsRequired();

		builder.Property(review => review.Rating)
			.HasColumnType("char")
			.IsRequired();

		builder.Property(review => review.Title)
			.HasMaxLength(50);

		builder.Property(review => review.Comment)
			.HasMaxLength(1000);

		builder.Property(review => review.CreationDate);
	}
}
