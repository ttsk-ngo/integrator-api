using Integrator.Api.Errors;
using Integrator.Shared.ErrorHandling;

namespace Integrator.Api.HostingExtensions;

internal static class Helpers
{
    internal static string GetConfigurationValue(this IConfiguration configuration, string section)
    {
        var connectionString = configuration.GetSection(section).Value;
        if (connectionString == null)
        {
            throw new IntegratorException(ApiErrors.ConfigurationError);
        }

        return connectionString;
    }
}