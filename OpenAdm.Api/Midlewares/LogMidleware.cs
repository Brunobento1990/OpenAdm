using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Logs;
using OpenAdm.Application.Interfaces;
using System.Text.Json;

namespace OpenAdm.Api.Midlewares;

public class LogMidleware
{
    private readonly RequestDelegate _next;
    private CreateAppLog _createAppLog;
    private const string _erroGenerico =
        "Ocorreu um erro interno, tente novamente mais tarde!";

    public LogMidleware(RequestDelegate next)
    {
        _createAppLog = new();
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IAppLogService appLogService)
    {
        try
        {
            _createAppLog.Host = (string?)httpContext.Request.Headers.Host ?? "Host não localizado!";
            _createAppLog.Origem = (string?)httpContext.Request.Headers.FirstOrDefault(x => x.Key == "Referer").Value ?? "Referer não lozalizada!";
            _createAppLog.Path = httpContext.Request.Path;
            _createAppLog.Ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "Ip não lozalizado!";
            _createAppLog.LogLevel = 0;
            _createAppLog.StatusCode = 200;

            await _next(httpContext);
        }
        catch (ExceptionUnauthorize ex)
        {
            await HandleError(httpContext, ex.Message, 401);
            _createAppLog ??= new();
            _createAppLog.StatusCode = 401;
            _createAppLog.LogLevel = 1;
            _createAppLog.Erro = ex.Message;
        }
        catch (ExceptionApi ex)
        {
            await HandleError(httpContext, ex.Message, 404);
            _createAppLog ??= new();
            _createAppLog.StatusCode = 404;
            _createAppLog.Erro = ex.Message;
            _createAppLog.LogLevel = 2;
        }
        catch (Exception ex)
        {
            if (VariaveisDeAmbiente.IsDevelopment())
            {
                await HandleError(httpContext, ex.Message, 404);
            }
            else
            {
                await HandleError(
                    httpContext,
                    _erroGenerico,
                    404);
            }
            _createAppLog ??= new();
            _createAppLog.StatusCode = 404;
            _createAppLog.Erro = ex.Message;
            _createAppLog.LogLevel = 3;
        }
        finally
        {
            if (!VariaveisDeAmbiente.IsDevelopment())
            {
                await appLogService.CreateAppLogAsync(_createAppLog);
            }
        }
    }

    public async Task HandleError(HttpContext httpContext, string mensagem, int statusCode)
    {
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
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
    }
}
