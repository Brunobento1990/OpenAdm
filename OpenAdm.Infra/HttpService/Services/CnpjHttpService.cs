using Microsoft.Extensions.Configuration;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.HttpClient.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.Enums;
using OpenAdm.Infra.HttpService.Providers;
using Microsoft.Extensions.DependencyInjection;

namespace OpenAdm.Infra.HttpService.Services;

public class CnpjHttpService : IHttpClientConsultaCnpj
{
    private readonly IConfiguration _configuration;
    private readonly IServiceProvider _serviceProvider;
    private readonly ICachedService<ConsultaCnpjResponse> _cache;

    public CnpjHttpService(IConfiguration configuration, IServiceProvider serviceProvider,
        ICachedService<ConsultaCnpjResponse> cache)
    {
        _configuration = configuration;
        _serviceProvider = serviceProvider;
        _cache = cache;
    }

    public async Task<ConsultaCnpjResponse> ConsultarCnpjAsync(string cnpj)
    {
        var nomeProvider = _configuration["ApiConsultaCnpj:Provider"] ?? "MinhaReceita";

        if (!Enum.TryParse<ConsultaCnpjProviderEnum>(nomeProvider, true, out var providerKey))
        {
            throw new ExceptionApi($"Provider de consulta de CNPJ '{nomeProvider}' não configurado");
        }

        var cacheKey = $"consulta_cnpj_{providerKey}_{cnpj}";
        var resultadoCache = await _cache.GetItemAsync(cacheKey);

        if (resultadoCache != null)
        {
            return resultadoCache;
        }

        var provider = _serviceProvider.GetKeyedService<IConsultaCnpjProvider>(providerKey);

        if (provider == null)
        {
            throw new ExceptionApi($"Provider de consulta de CNPJ '{nomeProvider}' não registrado");
        }

        var resultado = await provider.ConsultarAsync(cnpj);
        await _cache.SetItemAsync(cacheKey, resultado);
        return resultado;
    }
}
