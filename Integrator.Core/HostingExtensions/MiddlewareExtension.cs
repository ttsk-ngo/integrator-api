using Integrator.Core.Essentials;

namespace Integrator.Core.HostingExtensions;

internal static class MiddlewareExtension
{
    internal static void AddScopedMiddlewares(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<GlobalExceptionMiddleware>();
    }

    internal static void UseMiddlewares(this WebApplication app)
    {
        app.UseMiddleware<GlobalExceptionMiddleware>();
    }
}