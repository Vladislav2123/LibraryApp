using LibraryApp.Application.Common.Mappings;
using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LibraryApp.Application
{
	public static class DependencyInjection
	{
		public static IServiceCollection AddApplication(this IServiceCollection services)
		{
			services.AddAutoMapper(cfg =>
			{
				cfg.AddProfile(new AssemblyMappingProfile(Assembly.GetExecutingAssembly()));
			});
			services.AddMediatR(cfg =>
			{
				cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
				cfg.NotificationPublisher = new ForeachAwaitPublisher();
			});


			return services;
		}
	}
}
