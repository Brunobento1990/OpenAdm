using OpenAdm.Domain.Errors;

namespace OpenAdm.Domain.Exceptions;

public class ExceptionApi(string? message = CodigoErrors.ErrorGeneric) 
    : Exception(message)
{
}
