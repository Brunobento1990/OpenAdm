using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Validations;

public class ValidationDecimal
{
    public static void ValidDecimalNullAndZero(decimal? value, string message = CodigoErrors.ErrorGeneric)
    {
        if (value == null || value.Value <= 0)
        {
            throw new ExceptionApi(message);
        }
    }
}
