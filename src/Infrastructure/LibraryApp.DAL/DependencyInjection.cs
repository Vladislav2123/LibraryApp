using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using LibraryApp.Application.Interfaces;

namespace LibraryApp.DAL
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddDal(this IServiceCollection service, IConfiguration configuration)
		{
			service.AddDbContext<LibraryDbContext>(options =>
			{
				options.UseSqlServer(configuration.GetConnectionString("SqlServer"));
			});
			service.AddScoped<ILibraryDbContext>(provider => provider.GetService<LibraryDbContext>());

			return service;
		}
	}
}
