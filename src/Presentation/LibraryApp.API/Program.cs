using LibraryApp.DAL;
using LibraryApp.Application;
using LibraryApp.API.Middleware;
using Serilog;
using LibraryApp.Domain.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddApplication()
	.AddTransient<GlobalExceptionHandlingMiddleware>()
	.AddDal(builder.Configuration);

builder.Services
	.Configure<FilePaths>(
		builder.Configuration
		.GetSection(FilePaths.ConfigSectionKey));

builder.Host.UseSerilog((context, congiguration) =>
	congiguration.ReadFrom.Configuration(context.Configuration));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider
		.GetRequiredService<LibraryDbContext>();
	DbInitializer.Initialize(dbContext);
}

app.Run();

