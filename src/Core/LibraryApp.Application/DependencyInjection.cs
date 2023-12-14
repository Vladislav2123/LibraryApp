using FluentValidation;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Common.Behaviours;
using LibraryApp.Application.Common.Mappings;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using LibraryApp.Application.Common.PasswordProviders;

namespace LibraryApp.Application;

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

		services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
		services.AddSingleton<IPasswordProvider, BcryptPasswordProvider>();
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}
