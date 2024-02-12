using Dep = LibraryApp.API.Authorization.DependencyInjection;
using LibraryApp.Application.Extensions.Environment;
using LibraryApp.Application.Abstractions;
using LibraryApp.API.ExceptionsHandling;
using LibraryApp.API.Authentication;
using LibraryApp.API.Authorization;
using LibraryApp.Application;
using LibraryApp.DAL;
using Serilog;
using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Configuration.AddUserSecrets<Program>();
builder.Services.AddControllers();
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddHttpContextAccessor()
	.AddApplication(builder.Configuration)
	.AddTransient<GlobalExceptionsHandlingMiddleware>()
	.AddTransient<CustomJwtValidationMiddleware>()
	.AddDal(builder.Configuration)
	.AddMyAuthentication(builder.Configuration)
	.AddMyAuthorization()
	.AddScoped<IJwtProvider, JwtProvider>();

builder.Host.UseSerilog((context, congiguration) =>
	congiguration.ReadFrom.Configuration(context.Configuration));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment() || app.Environment.IsTesting())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseGlobalExceptionsHandling();

app.UseAuthentication();
app.UseCustomJwtValidation();
app.UseAuthorization();

app.UseStaticFiles();
app.MapControllers();

await InitializeDb();

app.Run();


async Task InitializeDb()
{
	using (var scope = app.Services.CreateScope())
	{
		var dbContext = scope.ServiceProvider
		.GetService<ILibraryDbContext>();

		var mediator = scope.ServiceProvider
			.GetService<IMediator>();

		var fileWrapper = scope.ServiceProvider
			.GetService<IFileWrapper>();

		await DbInitializer.Initialize(
			builder.Configuration,
			dbContext,
			mediator,
			app.Environment,
			fileWrapper);
	}
}