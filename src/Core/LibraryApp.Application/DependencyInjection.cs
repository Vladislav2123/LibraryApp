﻿using FluentValidation;
using LibraryApp.Application.Abstractions;
using LibraryApp.Application.Mapping;
using MediatR;
using MediatR.NotificationPublishers;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using LibraryApp.Application.Common.PasswordProviders;
using LibraryApp.Application.Common.FileWrappers;
using LibraryApp.Application.Behaviours;

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
		services.AddSingleton<IFileWrapper, FileWrapper>();
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
		services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
		services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

		return services;
	}
}
