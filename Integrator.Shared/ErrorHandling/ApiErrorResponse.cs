namespace Integrator.Shared.ErrorHandling;

public class ApiErrorResponse(string statusCode, string reason, ICollection<string> messages)
{
    public ApiErrorResponse(string statusCode, string reason) : this(statusCode, reason, new List<string>())
    { }

    public ApiErrorResponse(IntegratorError error) : this(error.StatusCode, error.Reason, error.Messages)
    { }

    public string StatusCode { get; set; } = statusCode;
    public string Reason { get; set; } = reason;
    public ICollection<string> Messages { get; set; } = messages;
}