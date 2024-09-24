using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OpenAdm.Application.Services;

public static class ConfiguracaoPdfService
{
    public static void Configurar(this PageDescriptor page)
    {
        page.Size(PageSizes.A4);
        page.Margin(5, Unit.Millimetre);
        page.PageColor(Colors.White);
        page.DefaultTextStyle(x => x.FontSize(14));
    }

    public static void FooterCustom(this PageDescriptor page)
    {
        page.Footer()
                .AlignRight()
                .Height(20)
                .Text(x =>
                {
                    x.CurrentPageNumber().FontSize(8);
                    x.Span(" / ").FontSize(8);
                    x.TotalPages().FontSize(8);
                });
    }
}

