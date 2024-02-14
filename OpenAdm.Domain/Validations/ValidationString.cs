using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using System.Text.RegularExpressions;

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

    public static void ValidateLength(string? value, int length = 255, string message = GenericError.Error)
    {
        if (!string.IsNullOrWhiteSpace(value) && value.Length > length)
            throw new ExceptionApi(message);
    }

    public static void ValidateTelefone(string? value, string message = DomainErrorMessage.ErrorTelefoneInvalido)
    {
        const int length = 14;
        const string pattern = "^[0-9]+$";

        if (!string.IsNullOrWhiteSpace(value))
        {
            if(value.Length > length || !Regex.IsMatch(value, pattern))
                throw new ExceptionApi(message);
        }
    }
}
