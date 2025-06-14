using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Fretes;

namespace OpenAdm.Application.Services;

internal class ConsultaCepService : IConsultaCepService
{
    private readonly IHttpClientCep _httpClientCep;

    public ConsultaCepService(IHttpClientCep httpClientCep)
    {
        _httpClientCep = httpClientCep;
    }

    public async Task<EnderecoViewModel> ConsultarAsync(string cep)
    {
        var consulta = await _httpClientCep.ConsultaCepAsync(cep);
        return new EnderecoViewModel()
        {
            Bairro = consulta.Bairro,
            Complemento = consulta.Complemento ?? "",
            Localidade = consulta.Localidade,
            Logradouro = consulta.Logradouro,
            Uf = consulta.Uf,
            Cep = consulta.Cep.Replace("-", "")
        };
    }
}
