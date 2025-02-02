using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IHttpClientCep
{
    Task<CotacaoFreteResponse> CotarFreteAsync(CotacaoFreteRequest cotacaoFreteRequest);
    Task<ConsultaCepResponse> ConsultaCepAsync(string cepOrigem);
}
