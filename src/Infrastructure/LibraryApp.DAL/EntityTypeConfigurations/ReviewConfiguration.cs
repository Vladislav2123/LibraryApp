﻿using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.DAL.EntityTypeConfigurations
{
	public class ReviewConfiguration : IEntityTypeConfiguration<Review>
	{
		public void Configure(EntityTypeBuilder<Review> builder)
		{
			builder.Property(review => review.Rating)
				.HasPrecision(5)
				.IsRequired();

			builder.Property(review => review.Title)
				.HasMaxLength(50);

			builder.Property(review => review.Text)
				.HasMaxLength(1000);
		}
	}
}
