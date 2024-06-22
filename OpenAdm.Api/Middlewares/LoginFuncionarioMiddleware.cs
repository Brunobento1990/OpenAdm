using Domain.Pkg.Cryptography;
using Domain.Pkg.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Middlewares;

public class LoginFuncionarioMiddleware
{
    private readonly RequestDelegate _next;

    public LoginFuncionarioMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IConfiguracaoParceiroRepository configuracaoParceiroRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        if(httpContext.Request.Path == "/login/funcionario")
        {
            var referer = (string?)httpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value;

            if (string.IsNullOrEmpty(referer))
            {
                throw new ExceptionUnauthorize("O parceiro não foi localizado!");
            }

            var parceiro = await configuracaoParceiroRepository.GetParceiroByDominioAdmAsync(referer);

            if (parceiro == null || !parceiro.Ativo)
            {
                throw new ExceptionUnauthorize("Parceiro não autenticado!");
            }

            parceiroAutenticado.XApi = parceiro.XApi;
            parceiroAutenticado.StringConnection = CryptographyGeneric.Decrypt(parceiro.ConexaoDb);
        }

        await _next(httpContext);
    }
}
