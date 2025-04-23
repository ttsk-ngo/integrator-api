using Integrator.DataAccess.DbContexts;
using Integrator.DataAccess.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Integrator.Frontend.HostingExtensions;

public static class IdentityExtension
{
    internal static IServiceCollection AddIntegratorIdentity(this IServiceCollection services)
    {
        services.AddIdentity<IntegratorUser, IdentityRole>()
            .AddEntityFrameworkStores<IntegratorIdentityDbContext>();

        services.Configure<IdentityOptions>(options =>
        {
            options.Password.RequireDigit = true;
        });

        services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.HttpOnly = true;
            options.LoginPath = "/auth/login";
        });
        return services;
    }

    internal static WebApplicationBuilder AddIntegratorIdentityDatabase(this WebApplicationBuilder builder)
    {
        var configuration = builder.Configuration;
        var services = builder.Services;
        
        var serverVersion = configuration.GetSection("DatabaseConnection:Identity")["ServerVersion"];
        var connectionString = configuration.GetSection("DatabaseConnection:Identity")["ConnectionString"];
        services.AddDbContext<IntegratorIdentityDbContext>(options =>
            options.UseMySql(connectionString, new MySqlServerVersion(serverVersion), opt => opt.MigrationsAssembly("Integrator.Frontend")));
        return builder;
    }
}