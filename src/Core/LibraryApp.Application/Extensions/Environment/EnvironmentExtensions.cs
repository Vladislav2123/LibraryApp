using Microsoft.Extensions.Hosting;

namespace LibraryApp.Application.Extensions.Environment;

public static class EnvironmentExtensions
{
    public static bool IsTesting(this IHostEnvironment environment)
    {
        if(environment == null) throw new ArgumentNullException(nameof(environment));

        return environment.IsEnvironment(Environments.Testing);
    }
}
