using Integrator.DataAccess.DbContexts;
using Integrator.DataAccess.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Integrator.Api.HostingExtensions;

internal static class IdentityExtension
{
    internal static void AddIdentity(this WebApplicationBuilder webApplicationBuilder)
    {
        var identityDbServerVersion = new MySqlServerVersion(webApplicationBuilder.Configuration.GetConfigurationValue("DatabaseConnection:Identity:ServerVersion"));
        var connectionString = webApplicationBuilder.Configuration.GetConfigurationValue("DatabaseConnection:Identity:ConnectionString");
        webApplicationBuilder.Services.AddDbContext<IntegratorIdentityDbContext>(options => 
            options.UseMySql(connectionString, identityDbServerVersion, mysqlOptions => mysqlOptions.MigrationsAssembly("Integrator.Api")));
        
        webApplicationBuilder.Services.AddIdentity<IntegratorUser, IdentityRole>()
            .AddEntityFrameworkStores<IntegratorIdentityDbContext>()
            .AddDefaultTokenProviders();
    }
}