using Integrator.Shared.ErrorHandling;
using Newtonsoft.Json;
using Serilog;

namespace Integrator.Api.Middlewares;

internal class GlobalExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (IntegratorException exception)
        {
            await SendResponse(context, exception.Error.HttpStatusCode, new ApiErrorResponse(exception.Error));
        }
        catch (Exception exception)
        {
            Log.Error("An unhandled exception was caught: {ExceptionMessage}", exception);
            await SendResponse(context, 500, new ApiErrorResponse(GlobalErrors.InternalServerError));
        }
    }

    private static async Task SendResponse(HttpContext context, int httpStatusCode, ApiErrorResponse response)
    {
        context.Response.StatusCode = httpStatusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(JsonConvert.SerializeObject(response));
        Log.Information("Sent response {HttpStatusCode}: {Reason}", httpStatusCode, response.Reason);
    }
}