using OpenAdm.Application.Attributes;
using OpenAdm.Domain.Constants;

namespace OpenAdm.Application.Dtos.Representantes;

public sealed class EditarRepresentanteDto : ValidarBaseDTO
{
    public Guid Id { get; set; }

    [ValidaString(erro: "Informe o nome", maxLength: RepresentanteConstantes.NomeMaxLength)]
    public string Nome { get; set; } = string.Empty;

    [ValidaStringLength(maxLength: RepresentanteConstantes.CpfDtoMaxLength)]
    [ValidaCpf]
    public string? Cpf { get; set; }

    [ValidaStringLength(maxLength: RepresentanteConstantes.EmailMaxLength)]
    [ValidaEmail]
    public string? Email { get; set; }

    [ValidaStringLength(maxLength: RepresentanteConstantes.TelefoneMaxLength)]
    public string? Telefone { get; set; }

}
