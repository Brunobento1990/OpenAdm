using OpenAdm.Infra.Model;

namespace OpenAdm.Application.Interfaces;

public interface ICnpjConsultaService
{
    Task<ConsultaCnpjResponse> ConsultaCnpjAsync(string cnpj);
}
