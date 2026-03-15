using Microsoft.IdentityModel.Tokens;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using OpenAdm.Api.Extensions;

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
        IUsuarioAutenticado usuarioAutenticado)
    {
        if (!httpContext.TemAtributo<AutenticaAttribute>())
        {
            await _next(httpContext);
            return;
        }

        var token = httpContext.Request.Headers.Authorization.ToString().Split(" ").Last();

        if (string.IsNullOrWhiteSpace(token))
        {
            await httpContext.RetornarErro("Por favor, efetue o login novamente",
                httpStatusCode: HttpStatusCode.Unauthorized);
            return;
        }

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = ConfiguracaoDeToken.Issue,
                ValidAudience = ConfiguracaoDeToken.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(ConfiguracaoDeToken.Key))
            }, out SecurityToken validatedToken);

            var jwtToken = (JwtSecurityToken)validatedToken;

            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value;
            var isFuncionario = jwtToken.Claims.FirstOrDefault(c => c.Type == "IsFuncionario")?.Value;

            if (!Guid.TryParse(id, out Guid idParse))
            {
                await httpContext.RetornarErro("Por favor, efetue o login novamente",
                    httpStatusCode: HttpStatusCode.Unauthorized);
                return;
            }

            usuarioAutenticado.Id = idParse;
            usuarioAutenticado.IsFuncionario = isFuncionario == "TRUE";
        }
        catch (SecurityTokenExpiredException)
        {
            await httpContext.RetornarErro("Sessão expirada, efetue o login novamente!",
                httpStatusCode: HttpStatusCode.Unauthorized);
            return;
        }
        catch (Exception)
        {
            await httpContext.RetornarErro("Efetue o login novamente!",
                httpStatusCode: HttpStatusCode.Unauthorized);
            return;
        }

        await _next(httpContext);
    }
}