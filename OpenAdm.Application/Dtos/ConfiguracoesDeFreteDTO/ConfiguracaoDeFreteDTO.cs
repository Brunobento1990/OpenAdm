using OpenAdm.Application.Attributes;

namespace OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;

public class ConfiguracaoDeFreteDTO : ValidarBaseDTO
{
    [ValidaString(erro: "Informe o token", maxLength: 2000,
        erroMaxLength: "O token deve conter no máximo 2000 caracteres")]
    public string Token { get; set; } = string.Empty;

    public bool Ativo { get; set; }
    public bool CobrarCnpj { get; set; }
    public bool CobrarCpf { get; set; }

    [ValidaStringLength(maxLength: 8, erroMaxLength: "O CEP deve conter no máximo 8 caracteres")]
    public string? CepOrigem { get; set; }
}