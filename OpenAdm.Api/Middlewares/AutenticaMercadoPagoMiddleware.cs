using OpenAdm.Api.Attributes;
using OpenAdm.Api.Extensions;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using Serilog;

namespace OpenAdm.Api.Middlewares;

public class AutenticaMercadoPagoMiddleware
{
    private readonly RequestDelegate _next;

    public AutenticaMercadoPagoMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(
        HttpContext httpContext,
        IEmpresaOpenAdmRepository empresaOpenAdmRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        if (!httpContext.TemAtributo<AutenticaMercadoPagoAttribute>())
        {
            await _next(httpContext);
            return;
        }

        var header = httpContext.Request.Headers["X-Signature"].FirstOrDefault();
        var header2 = httpContext.Request.Headers["x-signature"].FirstOrDefault();

        if (string.IsNullOrWhiteSpace(header) && string.IsNullOrWhiteSpace(header2))
        {
            Log.Warning("Não passou porque não pegou os signatures");
            return;
        }

        var parceiroIdString = httpContext.Request.QueryString.Value;

        if (!string.IsNullOrEmpty(parceiroIdString) || !Guid.TryParse(parceiroIdString, out Guid parceiroId))
        {
            Log.Warning($"Não passou porque não pegou o parceiroId: {parceiroIdString}");
            return;
        }

        var empresaOpenAdm = await empresaOpenAdmRepository.ObterPorIdAsync(parceiroId);

        if (empresaOpenAdm == null)
        {
            Log.Warning("Não passou porque não pegou o parceiro");
            return;
        }

        parceiroAutenticado.Id = empresaOpenAdm.Id;
        parceiroAutenticado.ConnectionString = Criptografia.Decrypt(empresaOpenAdm.ConnectionString);

        await _next(httpContext);
    }
}