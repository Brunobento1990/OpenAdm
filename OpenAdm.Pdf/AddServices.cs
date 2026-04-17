using Microsoft.Extensions.DependencyInjection;
using OpenAdm.Pdf.Interfaces;
using OpenAdm.Pdf.Services;
using QuestPDF.Infrastructure;

namespace OpenAdm.Pdf;

public static class AddServices
{
    public static IServiceCollection ConfigurarPdf(this IServiceCollection services)
    {
        QuestPDF.Settings.License = LicenseType.Community;

        services.AddScoped<IPdfPedidoService, PdfPedidoService>();
        services.AddScoped<IMovimentacaoDeProdutoRelatorioService, MovimentacaoDeProdutoRelatorioService>();

        return services;
    }
}
