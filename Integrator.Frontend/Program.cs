using Integrator.Frontend.Components;
using Integrator.Frontend.HostingExtensions;
using Integrator.Frontend.Services.Auth;
using Integrator.Frontend.Services.Login;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components.Authorization;
using RestSharp;
using Serilog;
using AuthenticationService = Integrator.Frontend.Services.Auth.AuthenticationService;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = LoggingExtension.GetSerilogBootstrapLogger();
    Log.Information("Starting up");
    Log.Information("Selected environment: {Environment}", builder.Environment.EnvironmentName);

    builder.AddScopedMiddlewares();

    builder.ConfigureSerilogLogger();
    
    builder.Services.AddLocalAndOidcAuthentication();
    
    builder.Services.AddScoped(_ => new RestClient(new HttpClient()));

    builder.Services.AddScoped<ILoginService, LoginService>();

    builder.Services.AddScoped<AuthenticationService>();
    builder.Services.AddScoped<AuthenticationStateProvider, IntegratorAuthenticationStateProvider>();
    
    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    var app = builder.Build();

    app.UseStatusCodePagesWithReExecute("/errors/{0}");
    
    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Error", createScopeForErrors: true);
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseMiddlewares();

    app.UseHttpsRedirection();

    app.UseStaticFiles();
    app.UseAntiforgery();

    app.MapRazorComponents<App>()
        .AddInteractiveServerRenderMode();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Unhandled exception during startup");
}
finally
{
    Log.Information("Shut down complete");
    Log.CloseAndFlush();
}