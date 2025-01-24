using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;

namespace OpenAdm.Application.Services;

public class CnpjConsultaService : ICnpjConsultaService
{
    private readonly ICnpjHttpService _cnpjHttpService;

    public CnpjConsultaService(ICnpjHttpService cnpjHttpService)
    {
        _cnpjHttpService = cnpjHttpService;
    }

    public async Task<ConsultaCnpjResponse> ConsultaCnpjAsync(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            throw new ExceptionApi("CNPJ inválido");
        }

        return await _cnpjHttpService.ConsultarCnpjAsync(cnpj);
    }
}
