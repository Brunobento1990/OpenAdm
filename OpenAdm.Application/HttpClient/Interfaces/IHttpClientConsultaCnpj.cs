using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Application.HttpClient.Interfaces;

public interface IHttpClientConsultaCnpj
{
    Task<ConsultaCnpjResponse> ConsultarCnpjAsync(string cnpj);
}
