using OpenAdm.Application.Dtos.Response;
using OpenAdm.Domain.Exceptions;
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

    public async Task Invoke(HttpContext httpContext)
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
                await HandleError(httpContext, ex.Message, ex);
            }
            else
            {
                await HandleError(
                    httpContext,
                    _erroGenerico, ex);
            }
        }
    }

    public async Task HandleError(HttpContext httpContext, string mensagem, Exception? ex = null)
    {
        httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = _statusCode;
        var errorResponse = new ErrorResponse()
        {
            Mensagem = mensagem
        };

        await httpContext.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        if (ex != null)
        {
            Log.Error(ex, mensagem);
            return;
        }

        Log.Error(mensagem);
    }
}