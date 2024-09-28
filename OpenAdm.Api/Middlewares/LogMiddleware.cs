using OpenAdm.Application.Dtos.Response;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;
using System.Text.Json;

namespace OpenAdm.Api.Midlewares;

public class LogMiddleware
{
    private readonly RequestDelegate _next;
    private const string _erroGenerico =
        "Ocorreu um erro interno, tente novamente mais tarde!";
    private readonly bool _development = VariaveisDeAmbiente.IsDevelopment();
    private int _statusCode = 200;

    public LogMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IDiscordHttpService discordHttpService)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ExceptionUnauthorize ex)
        {
            _statusCode = 401;
            await HandleError(httpContext, ex.Message);
        }
        catch (ExceptionApi ex)
        {
            _statusCode = 400;
            await HandleError(httpContext, ex.Message);
        }
        catch (Exception ex)
        {
            _statusCode = 400;

            if (_development)
            {
                await HandleError(httpContext, ex.Message);
            }
            else
            {
                await NotificarDiscord(httpContext, ex.Message, discordHttpService);
                await HandleError(
                    httpContext,
                    _erroGenerico);
            }
        }
    }

    static async Task NotificarDiscord(HttpContext httpContext, string mensagem, IDiscordHttpService discordHttpService)
    {
        var webHookId = VariaveisDeAmbiente.GetVariavel("DISCORD_WEB_HOOK_ID");
        var webHookToken = VariaveisDeAmbiente.GetVariavel("DISCORD_WEB_HOOK_TOKEN");

        var discordModel = new DiscordModel()
        {
            Content = "Error expeptions",
            Username = "Error",
            Embeds =
            [
                new()
                {
                    Description = mensagem,
                    Title = "Error api",
                    Color = 0xFF0000
                }
            ]
        };

        await discordHttpService.NotifyExceptionAsync(discordModel, webHookId, webHookToken);
    }

    public async Task HandleError(HttpContext httpContext, string mensagem)
    {
        httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = _statusCode;
        var errorResponse = new ErrorResponse()
        {
            Mensagem = mensagem
        };
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
