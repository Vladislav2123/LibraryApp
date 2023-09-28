using Microsoft.EntityFrameworkCore;
using LibraryApp.Domain.Enteties;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryApp.DAL.EntityTypeConfigurations
{
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			
		}
	}
}
