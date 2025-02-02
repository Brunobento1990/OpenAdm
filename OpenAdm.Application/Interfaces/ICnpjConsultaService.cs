using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Application.Interfaces;

public interface ICnpjConsultaService
{
    Task<ConsultaCnpjResponse> ConsultaCnpjAsync(string cnpj);
}
