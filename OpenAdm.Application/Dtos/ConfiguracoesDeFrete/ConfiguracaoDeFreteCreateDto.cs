using Domain.Pkg.Entities;

namespace OpenAdm.Application.Dtos.ConfiguracoesDeFrete;

public class ConfiguracaoDeFreteCreateDto
{
    public string CepOrigem { get; set; } = string.Empty;
    public string AlturaEmbalagem { get; set; } = string.Empty;
    public string LarguraEmbalagem { get; set; } = string.Empty;
    public string ComprimentoEmbalagem { get; set; } = string.Empty;
    public decimal? Peso { get; set; }

    public ConfiguracaoDeFrete ToEntity()
    {
        return new ConfiguracaoDeFrete(
            id: Guid.NewGuid(),
            dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now,
            numero:0,
            cepOrigem: CepOrigem,
            alturaEmbalagem: AlturaEmbalagem,
            larguraEmbalagem: LarguraEmbalagem,
            comprimentoEmbalagem: ComprimentoEmbalagem,
            peso: Peso);
    }
}
