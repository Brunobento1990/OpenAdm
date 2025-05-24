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
