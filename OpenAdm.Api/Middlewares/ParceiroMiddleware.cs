using Microsoft.AspNetCore.Http.Features;
using OpenAdm.Api.Attributes;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Midlewares;

public class ParceiroMiddleware
{
    private readonly RequestDelegate _next;
    public ParceiroMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IConfiguracaoParceiroRepository configuracaoParceiroRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        var autenticar = httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
                .FirstOrDefault(m => m is AutenticaParceiroAttribute) is AutenticaParceiroAttribute atributoAutorizacao;

        if (!autenticar)
        {
            await _next(httpContext);
            return;
        }

        var referer = (string?)httpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;

        if (string.IsNullOrWhiteSpace(referer))
        {
            throw new ExceptionUnauthorize("Referencia não localizada!!");
        }

        var parceiro = await configuracaoParceiroRepository.GetParceiroByDominioAdmAsync(referer);

        if (parceiro == null || !parceiro.Ativo)
        {
            throw new ExceptionUnauthorize("Parceiro não autenticado!");
        }

        parceiroAutenticado.Referer = referer;
        parceiroAutenticado.StringConnection = Criptografia.Decrypt(parceiro.ConexaoDb);
        parceiroAutenticado.KeyParceiro = parceiro.Id.ToString();
        parceiroAutenticado.NomeFantasia = parceiro.Parceiro.NomeFantasia;

        await _next(httpContext);
    }
}
