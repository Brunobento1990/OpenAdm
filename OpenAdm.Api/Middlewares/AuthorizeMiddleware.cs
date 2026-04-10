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
        ITokenService tokenService)
    {
        if (!httpContext.TemAtributo<AutenticaAttribute>())
        {
            await _next(httpContext);
            return;
        }

        if (!await httpContext.ValidarAcessoAsync(usuarioAutenticado, tokenService))
        {
            return;
        }

        await _next(httpContext);
    }
}