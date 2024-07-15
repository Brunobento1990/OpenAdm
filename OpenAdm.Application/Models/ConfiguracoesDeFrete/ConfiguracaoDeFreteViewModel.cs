using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.ConfiguracoesDeFrete;

public class ConfiguracaoDeFreteViewModel : BaseModel
{
    public string CepOrigem { get; set; } = string.Empty;
    public string AlturaEmbalagem { get; set; } = string.Empty;
    public string LarguraEmbalagem { get; set; } = string.Empty;
    public string ComprimentoEmbalagem { get; set; } = string.Empty;
    public decimal? Peso { get; set; }

    public static explicit operator ConfiguracaoDeFreteViewModel(ConfiguracaoDeFrete configuracaoDeFrete)
    {
        return new ConfiguracaoDeFreteViewModel()
        {
            AlturaEmbalagem = configuracaoDeFrete.AlturaEmbalagem,
            CepOrigem = configuracaoDeFrete.CepOrigem,
            ComprimentoEmbalagem = configuracaoDeFrete.ComprimentoEmbalagem,
            DataDeAtualizacao = configuracaoDeFrete.DataDeAtualizacao,
            DataDeCriacao = configuracaoDeFrete.DataDeCriacao,
            Id = configuracaoDeFrete.Id,
            LarguraEmbalagem = configuracaoDeFrete.LarguraEmbalagem,
            Numero = configuracaoDeFrete.Numero,
            Peso = configuracaoDeFrete.Peso
        };
    }
}
