using System.Net;
using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Api.Middlewares;

public sealed class IsRepresentanteMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IUsuarioAutenticado usuarioAutenticado,
        ISessaoUsuarioRepository sessaoUsuarioRepository)
    {
        if (!httpContext.TemAtributo<IsRepresentanteAttribute>())
        {
            await next(httpContext);
            return;
        }

        if (usuarioAutenticado.TipoUsuario != TipoUsuario.Representante)
        {
            if (usuarioAutenticado.SessaoId != Guid.Empty)
                await sessaoUsuarioRepository.DerrubarSessaoAsync(usuarioAutenticado.SessaoId);

            await httpContext.RetornarErroAsync(
                "Você não tem permissão para acessar essa rota!",
                HttpStatusCode.Unauthorized);
            return;
        }

        await next(httpContext);
    }
}
