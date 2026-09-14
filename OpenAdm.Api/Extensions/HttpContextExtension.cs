using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http.Features;
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

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
        var result = JsonSerializer.Serialize(errorResponse, JsonSerializerOptionsApi.Options);
        await context.Response.WriteAsync(result);
    }

    public static async Task<bool> ValidarAcessoAsync(this HttpContext httpContext,
        IUsuarioAutenticado usuarioAutenticado,
        ITokenService tokenService,
        ISessaoUsuarioRepository sessaoUsuarioRepository,
        string token)
    {
        var resultadoToken = tokenService.ValidarToken(token);

        if (!string.IsNullOrWhiteSpace(resultadoToken.Error) || resultadoToken.Result == null)
        {
            await httpContext.RetornarErroAsync(resultadoToken.Error ?? "JWT inválido", HttpStatusCode.Unauthorized);
            return false;
        }

        if (resultadoToken.Result.Expirado)
        {
            resultadoToken = tokenService.ValidarToken(token, validaLifeTime: false);

            if (!string.IsNullOrWhiteSpace(resultadoToken.Error) || resultadoToken.Result == null)
            {
                await httpContext.RetornarErroAsync(resultadoToken.Error ?? "JWT inválido",
                    HttpStatusCode.Unauthorized);
                return false;
            }

            var dadosToken = resultadoToken.Result;
            var sessao = await sessaoUsuarioRepository.ObterAsync(
                dadosToken.SessaoId,
                dadosToken.Id,
                dadosToken.ParceiroId,
                dadosToken.EhFuncionario);

            if (sessao == null || !sessao.Ativa)
            {
                await httpContext.RetornarErroAsync(
                    "Sessão expirada, efetue o login novamente",
                    HttpStatusCode.Unauthorized);
                return false;
            }

            var novoToken = tokenService.GenerateToken(sessao);

            httpContext.Response.Headers.TryAdd("novotoken", novoToken);
        }

        if (resultadoToken.Result.ParceiroId != usuarioAutenticado.ParceiroId)
        {
            await httpContext.RetornarErroAsync("JWT inválido para o parceiro informado",
                HttpStatusCode.Unauthorized);
            return false;
        }

        usuarioAutenticado.Id = resultadoToken.Result.Id;
        usuarioAutenticado.SessaoId = resultadoToken.Result.SessaoId;
        usuarioAutenticado.IsFuncionario = resultadoToken.Result.EhFuncionario;

        if (!usuarioAutenticado.IsFuncionario)
        {
            var usuario = await usuarioAutenticado.GetUsuarioMiddlewareAsync();

            if (usuario.ForcarLogin.HasValue &&
                usuario.ForcarLogin.Value > resultadoToken.Result.DataDoLogin)
            {
                await httpContext.RetornarErroAsync(
                    "Você foi forçado a efetuar o login novamente!",
                    HttpStatusCode.Unauthorized);
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
