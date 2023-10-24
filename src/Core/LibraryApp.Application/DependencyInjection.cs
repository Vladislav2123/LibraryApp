using LibraryApp.Application.Common.Mappings;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LibraryApp.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			Console.WriteLine("Adding auto mapper");
			services.AddAutoMapper(cfg =>
			{
				cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
			});
			services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));


			return services;
		}
	}
}
