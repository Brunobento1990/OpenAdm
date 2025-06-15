using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Domain.Entities;

public sealed class Parceiro : BaseEntity
{
    public Parceiro(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string razaoSocial,
        string nomeFantasia,
        string cnpj,
        byte[]? logo,
        Guid empresaOpenAdmId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj;
        Logo = logo;
        EmpresaOpenAdmId = empresaOpenAdmId;
    }

    public Guid EmpresaOpenAdmId { get; private set; }
    public EmpresaOpenAdm EmpresaOpenAdm { get; set; } = null!;
    public EnderecoParceiro? EnderecoParceiro { get; set; }
    public string RazaoSocial { get; private set; }
    public byte[]? Logo { get; private set; }
    public string NomeFantasia { get; private set; }
    public string Cnpj { get; private set; }
    public IList<RedeSocial> RedesSociais { get; set; } = [];
    public IList<TelefoneParceiro> Telefones { get; set; } = [];

    public void AddRedeSocial(string link, RedeSocialEnum redeSocialEnum)
    {
        RedesSociais.Add(RedeSocial.NovaRedeSocial(link, redeSocialEnum, Id));
    }

    public void AddTelefone(string telefone)
    {
        Telefones.Add(TelefoneParceiro.NovoTelefone(telefone.LimparMascaraTelefone(), Id));
    }

    public void Editar(
        string razaoSocial,
        string nomeFantasia,
        string cnpj,
        string? logo)
    {
        RazaoSocial = razaoSocial;
        NomeFantasia = nomeFantasia;
        Cnpj = cnpj.LimparMascaraCnpj();
        Logo = logo.ParaBytes();
    }

    //public ConfiguracaoParceiro ConfiguracaoDbParceiro { get; set; } = null!;
}
