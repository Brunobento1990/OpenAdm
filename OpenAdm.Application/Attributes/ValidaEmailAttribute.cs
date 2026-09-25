using System.Net.Mail;

namespace OpenAdm.Application.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ValidaEmailAttribute : ValidaBaseAttribute
{
    public override string? Validar(object? valor)
    {
        var email = valor?.ToString();
        return string.IsNullOrWhiteSpace(email) || MailAddress.TryCreate(email, out _)
            ? null
            : "Informe um e-mail válido";
    }
}
