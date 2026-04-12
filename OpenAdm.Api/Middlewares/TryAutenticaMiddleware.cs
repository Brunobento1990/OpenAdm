using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Middlewares;

public class TryAutenticaMiddleware
{
    private readonly RequestDelegate _next;

    public TryAutenticaMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IUsuarioAutenticado usuarioAutenticado,
        ITokenService tokenService)
    {
        if (usuarioAutenticado.Id != Guid.Empty)
        {
            await _next(httpContext);
            return;
        }

        if (!httpContext.TemAtributo<TryAutenticaAttribute>())
        {
            await _next(httpContext);
            return;
        }

        var token = httpContext.Request.Headers.Authorization.ToString().Split(" ").LastOrDefault();
        var refreshToken = httpContext.Request.Headers["refreshToken"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshToken))
        {
            await _next(httpContext);
            return;
        }

        if (!await httpContext.ValidarAcessoAsync(usuarioAutenticado, tokenService, token, refreshToken))
        {
            return;
        }

        await _next(httpContext);
    }
}