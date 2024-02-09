using OpenAdm.Domain.Errors;

namespace OpenAdm.Domain.Exceptions;

public class ExceptionApi(string? message = GenericError.Error) 
    : Exception(message)
{
}
