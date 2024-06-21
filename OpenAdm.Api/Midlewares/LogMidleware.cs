using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Logs;
using OpenAdm.Application.Interfaces;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;
using System.Text.Json;

namespace OpenAdm.Api.Midlewares;

public class LogMidleware
{
    private readonly RequestDelegate _next;
    private CreateAppLog _createAppLog;
    private const string _erroGenerico =
        "Ocorreu um erro interno, tente novamente mais tarde!";
    private readonly bool _development = VariaveisDeAmbiente.IsDevelopment();
    private int _statusCode = 200;

    public LogMidleware(RequestDelegate next)
    {
        _createAppLog = new();
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IAppLogService appLogService,
        IDiscordHttpService discordHttpService)
    {
        try
        {
            var xApiHeader = (string?)httpContext.Request.Headers.FirstOrDefault(x => x.Key == "X-Api").Value;
            _ = Guid.TryParse(xApiHeader, out Guid xApi);
            _createAppLog.XApi = xApi;
            _createAppLog.Host = (string?)httpContext.Request.Headers.Host ?? "Host não localizado!";
            _createAppLog.Origem = (string?)httpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value ?? "Referer não lozalizada!";
            _createAppLog.Path = httpContext.Request.Path;
            _createAppLog.Ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Ip não lozalizado!";
            _createAppLog.LogLevel = 0;
            _createAppLog.StatusCode = _statusCode;

            await _next(httpContext);
        }
        catch (ExceptionUnauthorize ex)
        {
            _statusCode = 401;
            await HandleError(httpContext, ex.Message);
            _createAppLog ??= new();
            _createAppLog.StatusCode = _statusCode;
            _createAppLog.LogLevel = 1;
            _createAppLog.Erro = ex.Message;
        }
        catch (ExceptionApi ex)
        {
            _statusCode = 404;
            await HandleError(httpContext, ex.Message);
            _createAppLog ??= new();
            _createAppLog.StatusCode = _statusCode;
            _createAppLog.Erro = ex.Message;
            _createAppLog.LogLevel = 2;
        }
        catch (Exception ex)
        {
            _statusCode = 404;

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

            _createAppLog ??= new();
            _createAppLog.StatusCode = _statusCode;
            _createAppLog.Erro = ex.Message;
            _createAppLog.LogLevel = 3;
        }
        finally
        {
            if (!_development)
            {
                await appLogService.CreateAppLogAsync(_createAppLog);
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
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = _statusCode;
        var errorResponse = new
        {
            Mensagem = mensagem
        };
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}


public static class AddLog
{
    public static void AddLogMain(this WebApplication app)
    {
        app.UseMiddleware<LogMidleware>();
        app.UseMiddleware<ParceiroMidleware>();
    }
}
