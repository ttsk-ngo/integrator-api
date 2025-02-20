using Serilog;
using Serilog.Extensions.Hosting;

namespace Integrator.Api.HostingExtensions;

internal static class LoggingExtension
{
    internal static void ConfigureSerilogLogger(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
            .GetBaseLoggerConfiguration()
            .ReadFrom.Configuration(ctx.Configuration)
        );
    }
    
    internal static ReloadableLogger GetSerilogBootstrapLogger()
    {
        return new LoggerConfiguration().GetBaseLoggerConfiguration().CreateBootstrapLogger();
    }

    private static LoggerConfiguration GetBaseLoggerConfiguration(this LoggerConfiguration loggerConfiguration)
    {
        return loggerConfiguration
            .WriteTo.Logger(x => x
                .WriteTo.File(
                    "Logs/Core/CR.log",
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId}] {SourceContext} {Message:lj}{NewLine}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14
                )
            )
            .WriteTo.Logger(x => x
                .MinimumLevel.Error()
                .WriteTo.File(
                    "Logs/Exception/EX.log",
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level:u3} {CorrelationId}] {SourceContext} {Message:lj}{NewLine}{Exception}",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 14
                )
            )
            .Enrich.FromLogContext()
            .Enrich.WithCorrelationIdHeader();
    }
}