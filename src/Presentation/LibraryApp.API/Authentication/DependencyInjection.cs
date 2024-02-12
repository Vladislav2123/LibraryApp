using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LibraryApp.API.Authentication;

public static class DependencyInjection
{
    public static IServiceCollection AddMyAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthenticationConfig>(
            configuration.GetSection(AuthenticationConfig.ConfigSectionKey));

        AuthenticationConfig? authConfig = 
            configuration.GetSection(AuthenticationConfig.ConfigSectionKey)
        	.Get<AuthenticationConfig>();

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        return services;
    }
}
