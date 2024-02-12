using LibraryApp.Application.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using LibraryApp.Application.Features.Users.Commands.Create;
using LibraryApp.Domain.Entities;
using LibraryApp.Application.Extensions.Environment;
using LibraryApp.Application.Features.Users.Commands.UpdateUserRole;

namespace LibraryApp.DAL;
public class DbInitializer
{
	private const int TestUsersAmount = 500;

	/// <summary>
	/// Initializes the database.
	/// </summary>
	public static async Task Initialize(
		IConfiguration configuration,
		ILibraryDbContext dbContext,
		IMediator mediator,
		IWebHostEnvironment environment,
		IFileWrapper fileWrapper)
	{
		if (environment.IsDevelopment() || environment.IsTesting())
			ClearDatabase(dbContext, fileWrapper);


		dbContext.Database.EnsureCreated();

		if (environment.IsTesting()) await SeedDatabaseTesting(configuration, dbContext, mediator);
		else await SeedDatabaseDefault(configuration, dbContext, mediator);

		Console.WriteLine($"Database seeded");
	}

    private static async Task SeedDatabaseTesting(
		IConfiguration configuration, 
		ILibraryDbContext dbContext, 
		IMediator mediator)
    {
		await CreateAdminUser(configuration, mediator);

		List<User> users = new(TestUsersAmount);
		Random random = new();
		for(int i = 1; i <= TestUsersAmount; i++)
		{
			users.Add(new()
			{
				Id = Guid.NewGuid(),
				Name = $"User {i}",
				Email = $"user_{i}@gmail.com",
				PasswordHash = $"Password_{i}",
				PasswordSalt = Guid.NewGuid().ToString(),
				BirthDate = DateOnly.Parse($"{random.Next(1970, 2010)}-01-01"),
				CreationDate = DateTime.Now,
				Role = UserRole.Default,
				AvatarPath = string.Empty
			});
		}


		await dbContext.Users.AddRangeAsync(users);
		dbContext.SaveChanges();
    }

    private static async Task SeedDatabaseDefault(
		IConfiguration configuration,
		ILibraryDbContext dbContext,
		IMediator mediator)
	{
		if (dbContext.Users.Any() == false)
			await CreateAdminUser(configuration, mediator);
	}

	private static async Task CreateAdminUser(
		IConfiguration config,
		IMediator mediator)
	{
		var createUserCommand = new CreateUserCommand(
			config["Admin:Name"],
			config["Admin:Email"],
			config["Admin:Password"],
			DateOnly.Parse(config["Admin:BirthDate"]));

		var adminUserId = await mediator.Send(createUserCommand);

		var updateUserRoleCommand = 
			new UpdateUserRoleCommand(adminUserId, UserRole.Admin.ToString());

		await mediator.Send(updateUserRoleCommand);
	}

	/// <summary>
	/// Deletes a database if there is any data.
	/// </summary>
	private static void ClearDatabase(
		ILibraryDbContext dbContext,
		IFileWrapper fileWrapper)
	{
		if (dbContext.Database.EnsureCreated() == true) return;

		foreach (var book in dbContext.Books
			.Select(book => new {book.ContentPath, book.CoverPath}))
		{
			fileWrapper.DeleteFile(book.ContentPath);
			fileWrapper.DeleteFile(book.CoverPath);
		}

		foreach (var author in dbContext.Authors
			.Select(author => new {author.AvatarPath})
			.Where(author => author.AvatarPath != string.Empty))
		{
			fileWrapper.DeleteFile(author.AvatarPath);
		}
			
		foreach (var user in dbContext.Users
			.Select(user => new {user.AvatarPath})
			.Where(user => user.AvatarPath != string.Empty))
		{
			fileWrapper.DeleteFile(user.AvatarPath);
		}

		dbContext.Database.EnsureDeleted();
	}
}
