namespace Integrator.Core.HostingExtensions;

internal static class HostingConfigurator
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.ConfigureSerilog();

        builder.Services.AddHeaderPropagation(x => x.Headers.Add("x-correlation-id"));
        
        builder.AddScopedMiddlewares();
        
        builder.Services.AddControllers().AddNewtonsoftJson();
        
        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseMiddlewares();

        app.MapControllers();
        
        return app;
    }
}