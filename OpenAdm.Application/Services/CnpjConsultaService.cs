using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Services;

public class CnpjConsultaService : ICnpjConsultaService
{
    private readonly IHttpClientConsultaCnpj _cnpjHttpService;

    public CnpjConsultaService(IHttpClientConsultaCnpj cnpjHttpService)
    {
        _cnpjHttpService = cnpjHttpService;
    }

    public async Task<ConsultaCnpjResponse> ConsultaCnpjAsync(string cnpj)
    {
        if (string.IsNullOrWhiteSpace(cnpj))
        {
            throw new ExceptionApi("CNPJ inválido");
        }

        var response = await _cnpjHttpService.ConsultarCnpjAsync(cnpj);

        if (string.IsNullOrWhiteSpace(response.NomeFantasia))
        {
            response.NomeFantasia = response.RazaoSocial;
        }

        return response;
    }
}
