using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Abstractions;

namespace LibraryApp.DAL;

public static class DependencyInjection
{
	public static IServiceCollection AddDal(this IServiceCollection service, IConfiguration configuration)
	{
		service.AddDbContext<LibraryDbContext>(options =>
		{
			Console.WriteLine("Connection to database");

			string host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
			string port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
			string db = Environment.GetEnvironmentVariable("DB_NAME") ?? "LibraryApp";
			string password = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? configuration["DbPassword"];


			options.UseNpgsql($"Host={host}; Port={port}; Database={db}; Username=postgres; Password={password}");
		});
		service.AddScoped<ILibraryDbContext>(provider => provider.GetService<LibraryDbContext>());

		return service;
	}
}
