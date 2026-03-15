using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IHttpClientCep
{
    Task<ConsultaCepResponse> ConsultaCepAsync(string cepOrigem);
}
