using OpenAdm.Application.HttpClient.Request;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IHttpClientFrete
{
    Task<ResultPartner<DadosCotacaoFreteResponse>> CotarFreteAsync(CotacaoFreteRequest request, string jwt);
}