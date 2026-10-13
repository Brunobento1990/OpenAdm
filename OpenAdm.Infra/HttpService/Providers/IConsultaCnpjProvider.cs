using OpenAdm.Application.HttpClient.Response;

namespace OpenAdm.Infra.HttpService.Providers;

public interface IConsultaCnpjProvider
{
    Task<ConsultaCnpjResponse> ConsultarAsync(string cnpj);
}
