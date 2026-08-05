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
            await HandleError(httpContext, ex.Message, 401);
        }
        catch (ExceptionApi ex)
        {
            await HandleError(httpContext, ex.Message, 400);
        }
        catch (Exception ex)
        {
            if (_development)
            {
                await HandleError(httpContext, ex.Message, 400, ex);
            }
            else
            {
                await HandleError(
                    httpContext,
                    _erroGenerico, 400, ex);
            }
        }
    }

    public async Task HandleError(HttpContext httpContext, string mensagem, int statusCode, Exception? ex = null)
    {
        httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = statusCode;
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