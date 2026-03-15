namespace OpenAdm.Application.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class ValidaStringAttribute : ValidaBaseAttribute
{
    private readonly string? _erro;
    private readonly string? _erroMaxLength;
    private readonly int _maxLength;

    public string Erro => _erro ?? "Campo obrigatório.";
    public string ErroMaxLength => _erroMaxLength ?? $"O campo deve ter no máximo {_maxLength} caracteres.";
    public int MaxLength => _maxLength;

    public ValidaStringAttribute(string? erro = null, string? erroMaxLength = null, int maxLength = 255)
    {
        _erro = erro;
        _maxLength = maxLength;
        _erroMaxLength = erroMaxLength;
    }

    public override string? Validar(object? valor)
    {
        var valorString = valor?.ToString();
        if (string.IsNullOrWhiteSpace(valorString))
        {
            return Erro;
        }

        if (valorString.Length > MaxLength)
        {
            return ErroMaxLength;
        }

        return null;
    }
}