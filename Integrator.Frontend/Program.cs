using Integrator.DataAccess.Models.Complaints;
using Integrator.Frontend.HostingExtensions;
using Integrator.Frontend.WebInterface;
using Integrator.Frontend.WebInterface.ViewModels.Complaints;
using Integrator.Frontend.WebInterface.ViewModels.Editor;
using MudBlazor.Extensions;
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

    // Integrator application database setup
    builder.AddIntegratorDatabase();

    builder.Services.AddScoped<IComplaintsViewModel, ComplaintsViewModel>();
    builder.Services.AddScoped<IRichTextEditorViewModel, RichTextEditorViewModel>();
    builder.Services.AddScoped<IDialogNewComplaintViewModel, DialogNewComplaintViewModel>();
    builder.Services.AddScoped<IComplaintDetailsViewModel, ComplaintDetailsViewModel>();

    builder.Services.AddSingleton<IComplaintsList, ComplaintsList>();

    builder.Services.AddCascadingAuthenticationState();
    
    // Add services to the container.
    builder.Services.AddRazorComponents()
        .AddInteractiveServerComponents();

    builder.Services.AddLocalization();
    // Add mudblazor
    builder.Services.AddMudServicesWithExtensions();

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