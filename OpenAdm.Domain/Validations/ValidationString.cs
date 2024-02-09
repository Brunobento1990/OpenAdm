using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Validations;

public class ValidationString
{
    public static void Validate(string? value, string message = GenericError.Error)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ExceptionApi(message);
    }

    public static void ValidateWithLength(string? value, int length = 255, string message = GenericError.Error)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length > length)
            throw new ExceptionApi(message);
    }
}
