using Integrator.Api.Middlewares;

namespace Integrator.Api.HostingExtensions;

internal static class MiddlewareConfigurator
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