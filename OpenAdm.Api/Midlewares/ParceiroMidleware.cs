using Domain.Pkg.Cryptography;
using Domain.Pkg.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Midlewares;

public class ParceiroMidleware
{
    private readonly RequestDelegate _next;
    public ParceiroMidleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IConfiguracaoParceiroRepository configuracaoParceiroRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        var xApiHeader = (string?)httpContext.Request.Headers.FirstOrDefault(x => x.Key == "X-Api").Value;
    
        var tryParseXApi = Guid.TryParse(xApiHeader, out Guid xApi);

        if (!tryParseXApi) 
        {
            throw new ExceptionUnauthorize("X-Api não localizada!");
        }

        var parceiro = await configuracaoParceiroRepository.GetConexaoDbByXApiAsync(xApi);

        if (parceiro == null || !parceiro.Ativo)
        {
            throw new ExceptionUnauthorize("Parceiro não autenticado!");
        }

        parceiroAutenticado.XApi = xApi;
        parceiroAutenticado.StringConnection = CryptographyGeneric.Decrypt(parceiro.ConexaoDb);

        await _next(httpContext);
    }
}
