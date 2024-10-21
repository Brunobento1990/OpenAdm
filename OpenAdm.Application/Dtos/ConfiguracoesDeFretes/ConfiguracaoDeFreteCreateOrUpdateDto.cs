namespace OpenAdm.Application.Dtos.ConfiguracoesDeFretes;

public class ConfiguracaoDeFreteCreateOrUpdateDto
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
}
