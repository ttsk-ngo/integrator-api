namespace Integrator.Frontend.HostingExtensions;

internal static class ApiHostingConfigurator
{
    public static void ConfigureApiServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers().AddNewtonsoftJson();
    }

    public static void ConfigureApiPipeline(this WebApplication app)
    {
        app.MapControllers();
    }
}