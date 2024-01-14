using LibraryApp.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using LibraryApp.Application.Features.Users.Commands.Create;
using LibraryApp.Domain.Entities;

namespace LibraryApp.DAL;

public class DbInitializer
{
	public static async Task Initialize(
		IConfiguration configuration,
		ILibraryDbContext dbContext,
		IMediator mediator,
		IWebHostEnvironment environment,
		IFileWrapper fileWrapper)
	{
		if (environment.IsDevelopment())
			ClearDatabase(dbContext, fileWrapper);

		dbContext.Database.EnsureCreated();

		await SeedDatabase(configuration, dbContext, mediator);
	}

	private static async Task SeedDatabase(
		IConfiguration config,
		ILibraryDbContext dbContext,
		IMediator mediator)
	{
		if (dbContext.Users.Any() == false)
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

	private static void ClearDatabase(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper)
	{
		if (dbContext.Database.EnsureCreated() == true) return;

		foreach (var book in dbContext.Books)
		{
			fileWrapper.DeleteFile(book.ContentPath);
			fileWrapper.DeleteFile(book.CoverPath);
		}

		foreach (var author in dbContext.Authors)
			fileWrapper.DeleteFile(author.AvatarPath);
			
		foreach (var user in dbContext.Users)
			fileWrapper.DeleteFile(user.AvatarPath);

		dbContext.Database.EnsureDeleted();
	}
}
