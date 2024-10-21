using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.ConfiguracoesDeFretes;

public class ConfiguracaoDeFreteViewModel : BaseModel
{
    public string CepOrigem { get; set; } = string.Empty;
    public string AlturaEmbalagem { get; set; } = string.Empty;
    public string LarguraEmbalagem { get; set; } = string.Empty;
    public string ComprimentoEmbalagem { get; set; } = string.Empty;
    public string ChaveApi { get; set; } = string.Empty;
    public int? Peso { get; set; }
    public bool? CobrarCpf { get; set; }
    public bool? CobrarCnpj { get; set; }
    public bool? Inativo { get; set; }

    public static explicit operator ConfiguracaoDeFreteViewModel(ConfiguracaoDeFrete configuracaoDeFrete)
    {
        return new ConfiguracaoDeFreteViewModel()
        {
            AlturaEmbalagem = configuracaoDeFrete.AlturaEmbalagem,
            CepOrigem = configuracaoDeFrete.CepOrigem,
            ChaveApi = configuracaoDeFrete.ChaveApi,
            CobrarCnpj = configuracaoDeFrete.CobrarCnpj,
            CobrarCpf = configuracaoDeFrete.CobrarCpf,
            ComprimentoEmbalagem = configuracaoDeFrete.ComprimentoEmbalagem,
            Id = configuracaoDeFrete.Id,
            LarguraEmbalagem = configuracaoDeFrete.LarguraEmbalagem,
            Peso = configuracaoDeFrete.Peso,
            Inativo = configuracaoDeFrete.Inativo
        };
    }
}
