using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Validations;

public static class ValidationGuid
{
    public static void ValidGuidNullAndEmpty(Guid? guid, string message = CodigoErrors.ErrorGeneric)
    {
        if (guid == null || guid.Value == Guid.Empty)
        {
            throw new ExceptionApi(message);
        }
    }
}
