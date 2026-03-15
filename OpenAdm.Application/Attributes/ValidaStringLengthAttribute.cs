namespace OpenAdm.Application.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ValidarStringLengthAttribute
{
    private readonly string? _erroMaxLength;
    private readonly int _maxLength;

    public string ErroMaxLength => _erroMaxLength ?? $"O campo deve ter no máximo {_maxLength} caracteres.";
    public int MaxLength => _maxLength;

    public ValidarStringLengthAttribute(string? erroMaxLength = null, int maxLength = 255)
    {
        _maxLength = maxLength;
        _erroMaxLength = erroMaxLength;
    }

    public string? Validar(string? valor)
    {
        if (valor?.Length > MaxLength)
        {
            return ErroMaxLength;
        }

        return null;
    }
}