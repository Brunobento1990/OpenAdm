using Microsoft.AspNetCore.Http.Features;
using OpenAdm.Api.Attributes;

namespace OpenAdm.Api.Middlewares;

public class AutenticaMercadoPagoMiddleware
{
    private readonly RequestDelegate _next;
    public AutenticaMercadoPagoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        var autenticar = httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
                .FirstOrDefault(m => m is AutenticaMercadoPagoAttribute) is AutenticaMercadoPagoAttribute atributoAutorizacao;

        if (!autenticar)
        {
            await _next(httpContext);
            return;
        }

        var header = httpContext.Request.Headers["X-Signature"].FirstOrDefault();
        var header2 = httpContext.Request.Headers["x-signature"].FirstOrDefault();

        Console.WriteLine($"header: {header}");
        Console.WriteLine($"header2: {header2}");

        if (string.IsNullOrWhiteSpace(header) && string.IsNullOrWhiteSpace(header2))
        {
            return;
        }

        Console.WriteLine("Processou o midleware com sucesso");
        await _next(httpContext);
    }
}
