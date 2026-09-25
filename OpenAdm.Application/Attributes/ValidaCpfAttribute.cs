using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Application.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public sealed class ValidaCpfAttribute : ValidaBaseAttribute
{
    public override string? Validar(object? valor)
    {
        var cpfNormalizado = valor?.ToString().LimparMascaraCpf();
        return string.IsNullOrWhiteSpace(cpfNormalizado) ||
               (cpfNormalizado.Length == 11 && cpfNormalizado.All(char.IsDigit) &&
                cpfNormalizado.Distinct().Count() > 1 &&
                ValidarCnpjECpf.IsCpf(cpfNormalizado))
            ? null
            : "Informe um CPF válido";
    }
}
