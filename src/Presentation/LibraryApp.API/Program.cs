using LibraryApp.DAL;
using LibraryApp.Application;
using Serilog;
using LibraryApp.Domain.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using LibraryApp.API.Authentication;
using LibraryApp.API.Authorization;
using LibraryApp.Application.Abstractions;
using MediatR;
using LibraryApp.Domain.Enteties;
using LibraryApp.API.Authorization.Role;
using LibraryApp.API.Authorization.ReviewEdit;
using LibraryApp.API.Authorization.UserEdit;
using LibraryApp.API.ExceptionsHandling;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddControllers();
builder.Services
	.AddEndpointsApiExplorer()
	.AddSwaggerGen()
	.AddHttpContextAccessor()
	.AddApplication()
	.AddTransient<GlobalExceptionsHandlingMiddleware>()
	.AddTransient<CustomJwtValidationMiddleware>()
	.AddDal(builder.Configuration)
	.AddPoliciesHandlers()
	.AddScoped<IJwtProvider, JwtProvider>();

builder.Host.UseSerilog((context, congiguration) =>
	congiguration.ReadFrom.Configuration(context.Configuration));

builder.Services
	.Configure<FilePaths>(
		builder.Configuration
		.GetSection(FilePaths.ConfigSectionKey));

builder.Services
	.Configure<AuthenticationConfig>(
		builder.Configuration
		.GetSection(AuthenticationConfig.ConfigSectionKey));

AuthenticationConfig? authConfig = builder.Configuration
	.GetSection(AuthenticationConfig.ConfigSectionKey)
	.Get<AuthenticationConfig>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters()
		{
			ValidIssuer = authConfig.Issuer,
			ValidAudience = authConfig.Audience,
			IssuerSigningKey = new SymmetricSecurityKey(
				Encoding.UTF8.GetBytes(authConfig.Key)),
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
		};
	});

builder.Services.AddAuthorization(options =>
{
	options.AddPolicy(Policies.AdminOnlyPolicyName, policy =>
		policy.Requirements.Add(new RoleRequirement(UserRole.Admin.ToString())));

	options.AddPolicy(Policies.ReviewUpdatePolicyName, policy =>
		policy.Requirements.Add(new EditReviewRequirement(false)));

	options.AddPolicy(Policies.ReviewDeletePolicyName, policy =>
		policy.Requirements.Add(new EditReviewRequirement(true)));

	options.AddPolicy(Policies.UserUpdatePolicyName, policy =>
		policy.Requirements.Add(new EditUserRequirement(false)));

	options.AddPolicy(Policies.UserDeletePolicyName, policy =>
		policy.Requirements.Add(new EditUserRequirement(true)));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
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

using (var scope = app.Services.CreateScope())
{
	var dbContext = scope.ServiceProvider
		.GetRequiredService<ILibraryDbContext>();

	var mediator = scope.ServiceProvider
		.GetRequiredService<IMediator>();

	await DbInitializer.Initialize(
		builder.Configuration, 
		dbContext, 
		mediator,
		app.Environment);
}

app.Run();

