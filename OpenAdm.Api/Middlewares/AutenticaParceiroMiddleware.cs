using Microsoft.AspNetCore.Http.Features;
using OpenAdm.Api.Attributes;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Middlewares;

public class AutenticaParceiroMiddleware
{
    private readonly RequestDelegate _next;
    public AutenticaParceiroMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IEmpresaOpenAdmRepository empresaOpenAdmRepository,
        IParceiroAutenticado parceiroAutenticado,
        IUsuarioAutenticado usuarioAutenticado)
    {
        var autenticar = httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
                .FirstOrDefault(m => m is AcessoParceiroAttribute) is AcessoParceiroAttribute atributoAutorizacao;

        if (!autenticar)
        {
            await _next(httpContext);
            return;
        }

        var origem = httpContext.Request.Headers.Origin;

        if (string.IsNullOrWhiteSpace(origem))
        {
            throw new ExceptionApi("Não foi possível autenticar a origem da requisição");
        }

        var empresaOpenAdm = await empresaOpenAdmRepository.ObterPorOrigemAsync(origem!)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro da empresa");

        parceiroAutenticado.Id = empresaOpenAdm.Id;
        usuarioAutenticado.ParceiroId = empresaOpenAdm.Id;
        parceiroAutenticado.ConnectionString = Criptografia.Decrypt(empresaOpenAdm.ConnectionString);

        await _next(httpContext);
    }
}
