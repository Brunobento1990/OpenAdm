namespace OpenAdm.Application.Attributes;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public abstract class ValidaBaseAttribute : Attribute
{
    public abstract string? Validar(object? valor);
}