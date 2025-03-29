using Integrator.Frontend.Components;
using Integrator.Frontend.HostingExtensions;
using Serilog;

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Logger = LoggingExtension.GetSerilogBootstrapLogger();
    Log.Information("Starting up");
    Log.Information("Selected environment: {Environment}", builder.Environment.EnvironmentName);

    builder.AddScopedMiddlewares();

    builder.ConfigureSerilogLogger();
    
    // Integrator identity database and setup
    builder.AddIntegratorIdentityDatabase();
    builder.Services.AddIntegratorIdentity();
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

    app.UseAuthentication();
    app.UseAuthorization();

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