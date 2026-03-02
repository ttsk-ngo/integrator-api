using Integrator.DataAccess.DbContexts;
using Microsoft.EntityFrameworkCore;

namespace Integrator.Frontend.HostingExtensions;

internal static class DatabaseExtension
{
    internal static WebApplicationBuilder AddIntegratorDatabase(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        
        var serverVersion = configuration.GetSection("DatabaseConnection:Application")["ServerVersion"];
        var connectionString = configuration.GetSection("DatabaseConnection:Application")["ConnectionString"];

        builder.Services.AddDbContextFactory<IntegratorDbContext>(options => options
            .UseMySql(connectionString, new MySqlServerVersion(serverVersion),
                opt => opt.MigrationsAssembly("Integrator.Frontend")));
        return builder;
    }
}