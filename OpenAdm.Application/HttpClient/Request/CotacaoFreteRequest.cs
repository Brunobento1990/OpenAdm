namespace OpenAdm.Application.HttpClient.Request;

public class CotacaoFreteRequest
{
    public string CepOrigem { get; set; } = string.Empty;
    public string CepDestino { get; set; } = string.Empty;
    public string Peso { get; set; } = string.Empty;
    public string Altura { get; set; } = string.Empty;
    public string Largura { get; set; } = string.Empty;
    public string Comprimento { get; set; } = string.Empty;
    public string ChaveDeAcesso { get; set; } = string.Empty;
}
