namespace OpenAdm.Domain.Exceptions;

public sealed class ExceptionApi : Exception
{
    public bool EnviarErroDiscord = false;
    public ExceptionApi(string message, bool enviarErroDiscord = false) : base(message)
    {
        EnviarErroDiscord = enviarErroDiscord;
    }
}
