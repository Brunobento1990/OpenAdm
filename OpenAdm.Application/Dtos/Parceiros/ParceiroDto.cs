using OpenAdm.Application.Attributes;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Application.Dtos.Parceiros;

public class ParceiroDto
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public IList<RedeSocialDto> RedesSociais { get; set; } = [];
    public IList<TelefoneDto> Telefones { get; set; } = [];
    public EnderecoDto? EnderecoParceiro { get; set; }

    public void Validar()
    {
        RazaoSocial
            .LimparMascaraCnpj()
            .ValidarNullOrEmpty("Informe a razão social")
            .ValidarLength(255, campo: "Razão social");
        NomeFantasia
            .ValidarNullOrEmpty("Informe nome fantasia")
            .ValidarLength(255, campo: "Nome fantasia");
        Cnpj
            .LimparMascaraCnpj()
            .ValidarNullOrEmpty("Informe o CNPJ")
            .ValidarLength(14, campo: "CNPJ");

        foreach (var item in Telefones)
        {
            item.Validar();
        }

        foreach (var item in RedesSociais)
        {
            item.Validar();
        }
    }
}

public class EnderecoDto : ValidarBaseDTO
{
    [ValidaString(erro: "Informe o CEP", erroMaxLength: "O CEP deve conter no máximo 8 caracteres", maxLength: 8)]
    public string Cep { get; set; } = string.Empty;

    [ValidaString(erro: "Informe a rua", erroMaxLength: "A rua deve conter no máximo 255 caracteres")]
    public string Logradouro { get; set; } = string.Empty;

    [ValidaString(erro: "Informe o bairro", erroMaxLength: "O bairro deve conter no máximo 255 caracteres")]
    public string Bairro { get; set; } = string.Empty;

    [ValidaString(erro: "Informe a cidade", erroMaxLength: "A cidade deve conter no máximo 255 caracteres")]
    public string Localidade { get; set; } = string.Empty;

    [ValidaStringLength(erroMaxLength: "O complemento deve conter no máximo 255 caracteres")]
    public string? Complemento { get; set; }

    [ValidaString(erro: "Informe o número", erroMaxLength: "O número deve conter no máximo 10 caracteres",
        maxLength: 10)]
    public string Numero { get; set; } = string.Empty;

    [ValidaString(erro: "Informe o estado", erroMaxLength: "O estado deve conter no máximo 2 caracteres",
        maxLength: 2)]
    public string Uf { get; set; } = string.Empty;
}

public class RedeSocialDto
{
    public Guid? Id { get; set; }
    public string Link { get; set; } = string.Empty;
    public RedeSocialEnum RedeSocialEnum { get; set; }

    public void Validar()
    {
        Link.ValidarNullOrEmpty("Informe o link da rede social")
            .ValidarLength(500);
    }
}

public class TelefoneDto
{
    public Guid? Id { get; set; }
    public string Telefone { get; set; } = string.Empty;

    public void Validar()
    {
        Telefone.ValidarNullOrEmpty("Informe o telefone")
            .LimparMascaraTelefone()
            .ValidarLength(14);
    }
}