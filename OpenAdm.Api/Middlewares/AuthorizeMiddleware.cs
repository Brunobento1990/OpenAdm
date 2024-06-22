using Domain.Pkg.Cryptography;
using Domain.Pkg.Exceptions;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.IdentityModel.Tokens;
using OpenAdm.Api.Attributes;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Domain.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

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
        if(httpContext.Request.Path != "/login/funcionario")
        {
            var token = httpContext.Request.Headers.Authorization.ToString().Split(" ").Last();
            var autenticar = httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
                    .FirstOrDefault(m => m is AutenticaAttribute) is AutenticaAttribute atributoAutorizacao;

            if (autenticar && string.IsNullOrWhiteSpace(token))
            {
                throw new ExceptionUnauthorize("Por favor, efetue o login novamente");
            }

            if (!string.IsNullOrWhiteSpace(token))
            {
                token = CryptographyGeneric.Decrypt(token);
                var keyJwt = VariaveisDeAmbiente.GetVariavel("JWT_KEY");

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

                    var id = jwtToken.Claims.FirstOrDefault(c => c.Type == "Id")?.Value
                        ?? throw new ExceptionUnauthorize("Token inválido");
                    var dataDeAtualizacao = jwtToken.Claims.FirstOrDefault(c => c.Type == "DataDeAtualizacao")?.Value
                        ?? throw new ExceptionUnauthorize("Token inválido");
                    var isFuncionario = jwtToken.Claims.FirstOrDefault(c => c.Type == "IsFuncionario")?.Value;

                    if (!DateTime.TryParse(dataDeAtualizacao, out DateTime newDataDeAtualizacao)
                        || !Guid.TryParse(id, out Guid idParse))
                    {
                        throw new ExceptionUnauthorize("Token inválido");
                    }

                    usuarioAutenticado.DataDeAtualizacao = newDataDeAtualizacao;
                    usuarioAutenticado.Id = idParse;
                    usuarioAutenticado.IsFuncionario = !string.IsNullOrWhiteSpace(isFuncionario) && isFuncionario == "TRUE";

                }
                catch (SecurityTokenExpiredException)
                {
                    throw new ExceptionUnauthorize("Sessão expirada, efetue o login novamente!");
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        await _next(httpContext);
    }
}
