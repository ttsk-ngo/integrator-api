namespace Integrator.Shared.ErrorHandling;

public abstract class IntegratorException(IntegratorError error) : Exception
{
    public IntegratorError Error { get; private set; } = error;
    public override string Message { get; } = error.Reason;
}