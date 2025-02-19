using System.Net;

namespace Integrator.Shared.ErrorHandling;

public class IntegratorError(HttpStatusCode httpStatusCode, string statusCode, string reason, ICollection<string> messages)
{
    public IntegratorError(HttpStatusCode httpStatusCode, string statusCode, string reason) : this(httpStatusCode, statusCode, reason, new List<string>())
    { }
    
    public int HttpStatusCode { get; set; } = (int)httpStatusCode;
    public string StatusCode { get; set; } = statusCode;
    public string Reason { get; set; } = reason;
    public ICollection<string> Messages { get; set; } = messages;
}