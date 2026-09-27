using OpenAdm.Application.Attributes;
using OpenAdm.Domain.Constants;

namespace OpenAdm.Application.Dtos.Representantes;

public sealed class LoginRepresentanteDto : ValidarBaseDTO
{
    [ValidaString(erro: "Informe o e-mail", maxLength: RepresentanteConstantes.EmailMaxLength)]
    [ValidaEmail]
    public string Email { get; set; } = string.Empty;

    [ValidaString(erro: "Informe a senha", maxLength: RepresentanteConstantes.SenhaDtoMaxLength)]
    public string Senha { get; set; } = string.Empty;
}
