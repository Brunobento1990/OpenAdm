using OpenAdm.Api.Attributes;
using OpenAdm.Domain.Interfaces;
using System.Net;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Interfaces;

namespace OpenAdm.Api.Midlewares;

public class AuthorizeMiddleware
{
    private readonly RequestDelegate _next;

    public AuthorizeMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IUsuarioAutenticado usuarioAutenticado,
        ITokenService tokenService,
        ISessaoUsuarioRepository sessaoUsuarioRepository)
    {
        if (!httpContext.TemAtributo<AutenticaAttribute>())
        {
            await _next(httpContext);
            return;
        }

        var token = httpContext.Request.Headers.Authorization.ToString().Split(" ").LastOrDefault();
        if (string.IsNullOrWhiteSpace(token))
        {
            await httpContext.RetornarErroAsync("Efetue o login", HttpStatusCode.Unauthorized);
            return;
        }

        if (!await httpContext.ValidarAcessoAsync(
                usuarioAutenticado, tokenService, sessaoUsuarioRepository, token))
        {
            return;
        }

        await _next(httpContext);
    }
}
