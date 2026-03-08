namespace OpenAdm.Domain.Exceptions;

public sealed class ExceptionApi : Exception
{
    public ExceptionApi(string message) : base(message)
    {
    }
}
