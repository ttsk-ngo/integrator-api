using Serilog;

namespace Integrator.Core.HostingExtensions;

internal static class LoggingExtension
{
    internal static void ConfigureSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((ctx, lc) => lc
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
            .Enrich.WithCorrelationIdHeader()
            .ReadFrom.Configuration(ctx.Configuration)
        );
    }
}