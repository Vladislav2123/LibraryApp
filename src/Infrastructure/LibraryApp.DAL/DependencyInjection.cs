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
			options.UseNpgsql("Host=localhost; Port=5432; Database=LibraryApp; Username=postgres; Password=Password123");
		});
		service.AddScoped<ILibraryDbContext>(provider => provider.GetService<LibraryDbContext>());

		return service;
	}
}
