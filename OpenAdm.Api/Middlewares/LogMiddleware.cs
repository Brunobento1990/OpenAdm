
using OpenAdm.Application.Dtos.Response;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Infra.HttpService.Interfaces;
using OpenAdm.Infra.Model;
using System.Text.Json;
using Serilog;

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
                await HandleError(
                    httpContext,
                    _erroGenerico);
            }
        }
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
        Log.Error(mensagem);
        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
    }
}
