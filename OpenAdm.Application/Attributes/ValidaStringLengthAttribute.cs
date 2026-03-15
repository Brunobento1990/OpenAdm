namespace OpenAdm.Application.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ValidaStringLengthAttribute : ValidaBaseAttribute
{
    private readonly string? _erroMaxLength;
    private readonly int _maxLength;

    public string ErroMaxLength => _erroMaxLength ?? $"O campo deve ter no máximo {_maxLength} caracteres.";
    public int MaxLength => _maxLength;

    public ValidaStringLengthAttribute(string? erroMaxLength = null, int maxLength = 255)
    {
        _maxLength = maxLength;
        _erroMaxLength = erroMaxLength;
    }

    public override string? Validar(object? valor)
    {
        if (valor?.ToString()?.Length > MaxLength)
        {
            return ErroMaxLength;
        }

        return null;
    }
}