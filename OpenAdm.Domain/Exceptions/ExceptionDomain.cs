using OpenAdm.Domain.Errors;

namespace OpenAdm.Domain.Exceptions;

public class ExceptionDomain(string? message = GenericError.Error) 
    : Exception(message)
{
}
