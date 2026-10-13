using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Infra.Enums;

namespace OpenAdm.Infra.HttpService.Providers;

public sealed class MinhaReceitaCnpjProvider : ConsultaCnpjProviderBase, IConsultaCnpjProvider
{
    private readonly IHttpClientFactory _httpClientFactory;

    public MinhaReceitaCnpjProvider(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<ConsultaCnpjResponse> ConsultarAsync(string cnpj)
    {
        using var client = _httpClientFactory.CreateClient(HttpServiceEnum.ConsultaCnpjMinhaReceita.ToString());
        using var response = await client.GetAsync(cnpj);
        var resultado = await ObterRespostaAsync<ConsultaCnpjResponse>(response);
        resultado.Ativa = resultado.DescricaoSituacaoCadastral.ToUpper() == "ATIVA";
        return resultado;
    }
}
