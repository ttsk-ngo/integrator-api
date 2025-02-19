using Integrator.Core.HostingExtensions;
using Serilog;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up");

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information("Selected environment: {Environment}", builder.Environment.EnvironmentName);

    var app = builder.ConfigureServices().ConfigurePipeline();
    
    app.Run();
}
catch (Exception)
{
    Log.Fatal("Unhandled exception during startup");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}