namespace OpenAdm.Api.Attributes;

[AttributeUsage(validOn: AttributeTargets.Class | AttributeTargets.Method)]
public sealed class IsRepresentanteAttribute : Attribute;
