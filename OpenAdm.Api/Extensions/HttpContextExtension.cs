using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Model;
using Serilog;

namespace OpenAdm.Api.Extensions;

public static class HttpContextExtension
{
    public static bool TemAtributo<T>(this HttpContext httpContext)
    {
        return httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
            .OfType<T>()
            .Any() ?? false;
    }

    public static async Task RetornarErroAsync(this HttpContext context, string erro,
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest)
    {
        var errorResponse = new ErrorResponse()
        {
            Mensagem = erro
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)httpStatusCode;
        var result = JsonSerializer.Serialize(errorResponse, JsonSerializerOptionsApi.Options());
        Log.Error(result, "Body response");
        await context.Response.WriteAsync(result);
    }

    public static async Task<bool> ValidarAcessoAsync(this HttpContext httpContext,
        IUsuarioAutenticado usuarioAutenticado,
        ITokenService tokenService)
    {
        var token = httpContext.Request.Headers.Authorization.ToString().Split(" ").LastOrDefault();
        var refreshToken = httpContext.Request.Headers["refreshToken"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(token) || string.IsNullOrWhiteSpace(refreshToken))
        {
            await httpContext.RetornarErroAsync("Por favor, efetue o login novamente",
                httpStatusCode: HttpStatusCode.Unauthorized);
            return false;
        }

        var resultadoToken = tokenService.ValidarToken(token);

        if (!string.IsNullOrWhiteSpace(resultadoToken.Error) || resultadoToken.Result == null)
        {
            await httpContext.RetornarErroAsync(resultadoToken.Error ?? "JWT inválido", HttpStatusCode.Unauthorized);
            return false;
        }

        if (resultadoToken.Result.Expirado)
        {
            resultadoToken = tokenService.ValidarToken(refreshToken);
            if (!string.IsNullOrWhiteSpace(resultadoToken.Error) || resultadoToken.Result == null)
            {
                await httpContext.RetornarErroAsync(resultadoToken.Error ?? "JWT inválido",
                    HttpStatusCode.Unauthorized);
                return false;
            }

            if (resultadoToken.Result.Expirado)
            {
                await httpContext.RetornarErroAsync("Sessão espirada, efetue o login novamente",
                    HttpStatusCode.Unauthorized);
                return false;
            }

            var novoToken = tokenService.GenerateToken(resultadoToken.Result.Id, resultadoToken.Result.EhFuncionario);

            httpContext.Response.Headers.TryAdd("novotoken", novoToken);
        }

        usuarioAutenticado.Id = resultadoToken.Result.Id;
        usuarioAutenticado.IsFuncionario = resultadoToken.Result.EhFuncionario;

        if (!usuarioAutenticado.IsFuncionario)
        {
            var usuario = await usuarioAutenticado.GetUsuarioMiddlewareAsync();

            if (usuario.ForcarLogin.HasValue &&
                (usuario.ForcarLogin.Value - resultadoToken.Result.DataDoLogin).TotalMinutes > 0)
            {
                await httpContext.RetornarErroAsync("Você foi forçado a efetuar o login novamente!",
                    httpStatusCode: HttpStatusCode.Unauthorized);
                return false;
            }

            if (!usuario.AcessoLiberadoEcommerce)
            {
                await httpContext.RetornarErroAsync("Seu acesso esta bloqueado!",
                    httpStatusCode: HttpStatusCode.Unauthorized);
                return false;
            }
        }

        return true;
    }
}