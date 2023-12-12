using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Feauters.Users.Commands.Create;
using LibraryApp.Domain.Enteties;
using MediatR;
using Microsoft.Extensions.Configuration;

namespace LibraryApp.DAL
{
	public class DbInitializer
	{
		public static async Task Initialize(
			IConfiguration configuration, 
			ILibraryDbContext dbContext, 
			IMediator mediator)
		{
			dbContext.Database.EnsureDeleted();
			dbContext.Database.EnsureCreated();

			await SeedDatabase(configuration, dbContext, mediator);
		}

		private static async Task SeedDatabase(
			IConfiguration config, 
			ILibraryDbContext dbContext, 
			IMediator mediator)
		{
			await CreateAdminUser(config, dbContext, mediator);

			dbContext.SaveChanges();
		}

		private static async Task CreateAdminUser(
			IConfiguration config,
			ILibraryDbContext dbContext,
			IMediator mediator)
		{
			var createUserCommand = new CreateUserCommand(
				config["Admin:Name"],
				config["Admin:Email"],
				config["Admin:Password"],
				DateOnly.Parse(config["Admin:BirthDate"]));

			var adminUserId = await mediator.Send(createUserCommand);

			var adminUser = dbContext.Users.FirstOrDefault(user => user.Id == adminUserId);
			adminUser.Role = UserRole.Admin;
		}
	}
}
