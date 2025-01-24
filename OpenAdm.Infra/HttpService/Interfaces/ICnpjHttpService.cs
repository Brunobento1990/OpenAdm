using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Interfaces;

public interface ICnpjHttpService
{
    Task<ConsultaCnpjResponse> ConsultarCnpjAsync(string cnpj);
}
