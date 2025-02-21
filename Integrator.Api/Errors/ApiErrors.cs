using System.Net;
using Integrator.Shared.ErrorHandling;

namespace Integrator.Api.Errors;

internal static class ApiErrors
{
    internal static readonly IntegratorError ConfigurationError = new IntegratorError(HttpStatusCode.InternalServerError, "02-101", "Missing or incorrect configuration");
}