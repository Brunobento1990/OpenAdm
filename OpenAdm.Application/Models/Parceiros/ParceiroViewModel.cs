using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Application.Models.Fretes;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Extensions;

namespace OpenAdm.Application.Models.Parceiros;

public class ParceiroViewModel : BaseViewModel
{
    public string RazaoSocial { get; set; } = string.Empty;
    public string NomeFantasia { get; set; } = string.Empty;
    public string Cnpj { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public IEnumerable<TelefoneParceiroViewModel> Telefones { get; set; } = [];
    public IEnumerable<RedeSocialViewModel> RedesSociais { get; set; } = [];
    public EnderecoViewModel? EnderecoParceiro { get; set; }

    public static explicit operator ParceiroViewModel(Parceiro parceiro)
    {
        return new ParceiroViewModel()
        {
            Id = parceiro.Id,
            DataDeCriacao = parceiro.DataDeCriacao,
            DataDeAtualizacao = parceiro.DataDeAtualizacao,
            RazaoSocial = parceiro.RazaoSocial,
            Cnpj = parceiro.Cnpj,
            Logo = parceiro.Logo.ParaString(),
            Telefones = parceiro.Telefones.Select(x => (TelefoneParceiroViewModel)x),
            NomeFantasia = parceiro.NomeFantasia,
            Numero = parceiro.Numero,
            RedesSociais = parceiro.RedesSociais.Select(x => (RedeSocialViewModel)x),
            EnderecoParceiro = parceiro.EnderecoParceiro == null ? null : new EnderecoViewModel()
            {
                Bairro = parceiro.EnderecoParceiro.Bairro,
                Cep = parceiro.EnderecoParceiro.Cep,
                Complemento = parceiro.EnderecoParceiro.Complemento,
                Localidade = parceiro.EnderecoParceiro.Localidade,
                Logradouro = parceiro.EnderecoParceiro.Logradouro,
                Uf = parceiro.EnderecoParceiro.Uf,
                Numero = parceiro.EnderecoParceiro.Numero
            }
        };
    }
}
