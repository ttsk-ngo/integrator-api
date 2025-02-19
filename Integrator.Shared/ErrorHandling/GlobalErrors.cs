using System.Net;

namespace Integrator.Shared.ErrorHandling;

public static class GlobalErrors
{
    public static readonly IntegratorError InternalServerError = new(HttpStatusCode.InternalServerError, "10-101", "Internal server error");
}