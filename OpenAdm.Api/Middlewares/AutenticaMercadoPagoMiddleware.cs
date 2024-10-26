using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.WebUtilities;
using OpenAdm.Api.Attributes;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Infra.Context;

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
        IConfiguracaoParceiroRepository configuracaoParceiroRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        var autenticar = httpContext.Features.Get<IEndpointFeature>()?.Endpoint?.Metadata
                .FirstOrDefault(m => m is AutenticaMercadoPagoAttribute) is AutenticaMercadoPagoAttribute atributoAutorizacao;

        if (!autenticar)
        {
            await _next(httpContext);
            return;
        }


        var request = httpContext.Request;
        var uriBuilder = new UriBuilder(request.Scheme, request.Host.Host)
        {
            Path = request.Path,
            Query = request.QueryString.ToString()
        };

        var query = QueryHelpers.ParseQuery(uriBuilder.Query);

        if (!query.ContainsKey("cliente"))
        {
            Console.WriteLine("Não encontrou a string da query params");
            return;
        }

        var cliente = query.FirstOrDefault(x => x.Key == "cliente").Value.ToString();
        var header = httpContext.Request.Headers["X-Signature"].FirstOrDefault();
        var header2 = httpContext.Request.Headers["x-signature"].FirstOrDefault();

        Console.WriteLine($"header: {header}");
        Console.WriteLine($"header2: {header2}");
        Console.WriteLine($"cliente: {cliente}");

        if (string.IsNullOrWhiteSpace(cliente) ||
            (string.IsNullOrWhiteSpace(header) && string.IsNullOrWhiteSpace(header2)))
        {
            return;
        }

        var configuracaoParceiro = await configuracaoParceiroRepository
            .GetParceiroAdmByMercadoPagoAsync(cliente);

        if (configuracaoParceiro == null)
        {
            return;
        }

        parceiroAutenticado.StringConnection = Criptografia.Decrypt(configuracaoParceiro.ConexaoDb);
        parceiroAutenticado.KeyParceiro = configuracaoParceiro.Parceiro.Id.ToString();
        parceiroAutenticado.NomeFantasia = configuracaoParceiro.Parceiro.NomeFantasia;

        Console.WriteLine("Processou o midleware com sucesso");
        await _next(httpContext);
    }
}
