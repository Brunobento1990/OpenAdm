using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Domain.Interfaces;
using UAParser;

namespace OpenAdm.Api.Middlewares;

public class UsuarioSessaoMiddleware
{
    private readonly RequestDelegate _next;
    private static readonly Parser _uaParser = Parser.GetDefault();

    public UsuarioSessaoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.TemAtributo<UsuarioSessaoAttribute>())
        {
            await _next(context);
            return;
        }

        var sessaoUsuarioRequest = context.RequestServices.GetRequiredService<IUsuarioSessaoRequest>();
        
        sessaoUsuarioRequest.UserAgent = context.Request.Headers.UserAgent.ToString();

        ClientInfo? info = string.IsNullOrWhiteSpace(sessaoUsuarioRequest.UserAgent)
            ? null
            : _uaParser.Parse(sessaoUsuarioRequest.UserAgent);

        sessaoUsuarioRequest.EnderecoIp = ExtrairIp(context);
        sessaoUsuarioRequest.SistemaOperacional = info?.OS.ToString();
        sessaoUsuarioRequest.Navegador = info?.UA.ToString();
        sessaoUsuarioRequest.Dispositivo = info?.Device.ToString();

        await _next(context);
    }

    private static string? ExtrairIp(HttpContext context)
    {
        var xForwardedFor = context.Request.Headers["X-Forwarded-For"].ToString();
        if (!string.IsNullOrWhiteSpace(xForwardedFor))
        {
            return xForwardedFor.Split(',')[0].Trim();
        }

        var xRealIp = context.Request.Headers["X-Real-IP"].ToString();
        if (!string.IsNullOrWhiteSpace(xRealIp))
        {
            return xRealIp.Trim();
        }

        return context.Connection.RemoteIpAddress?.ToString();
    }
}