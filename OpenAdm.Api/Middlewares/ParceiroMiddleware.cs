using Domain.Pkg.Cryptography;
using Domain.Pkg.Exceptions;
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
        parceiroAutenticado.StringConnection = CryptographyGeneric.Decrypt(parceiro.ConexaoDb);
        parceiroAutenticado.KeyParceiro = parceiro.Id.ToString();

        await _next(httpContext);
    }
}
