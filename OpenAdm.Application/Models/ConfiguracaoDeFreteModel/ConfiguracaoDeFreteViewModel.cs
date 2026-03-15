using OpenAdm.Application.Dtos.Bases;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.ConfiguracaoDeFreteModel;

public class ConfiguracaoDeFreteViewModel : BaseViewModel
{
    public string Token { get; set; } = string.Empty;
    public string? CepOrigem { get; set; }
    public bool Ativo { get; set; }
    public bool CobrarCnpj { get; set; }
    public bool CobrarCpf { get; set; }

    public static explicit operator ConfiguracaoDeFreteViewModel(ConfiguracaoDeFrete configuracaoDeFrete)
    {
        return new ConfiguracaoDeFreteViewModel()
        {
            Token = configuracaoDeFrete.Token,
            Id = configuracaoDeFrete.Id,
            DataDeAtualizacao = configuracaoDeFrete.DataDeAtualizacao,
            DataDeCriacao = configuracaoDeFrete.DataDeCriacao,
            Numero = configuracaoDeFrete.Numero,
            Ativo = configuracaoDeFrete.Ativo,
            CobrarCnpj = configuracaoDeFrete.CobrarCnpj,
            CobrarCpf = configuracaoDeFrete.CobrarCpf,
            CepOrigem = configuracaoDeFrete.CepOrigem,
        };
    }
}