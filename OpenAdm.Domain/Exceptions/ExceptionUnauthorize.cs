namespace OpenAdm.Domain.Exceptions;

public class ExceptionUnauthorize : Exception
{
    public ExceptionUnauthorize(string message) : base(message)
    {
    }
}
