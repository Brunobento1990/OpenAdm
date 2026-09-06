using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Model;
using OpenAdm.Pdf.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OpenAdm.Pdf.Services;

internal sealed class RelatorioVendaDeProdutoPdfService : IRelatorioVendaDeProdutoPdfService
{
    public byte[] Gerar(
        IEnumerable<RelatorioVendaDeProdutoModel> dados,
        string nomeFantasia,
        byte[]? logo,
        DateTime? dataInicial,
        DateTime? dataFinal,
        decimal quantidadeTotal,
        decimal valorTotal)
    {
        var itens = dados.ToList();

        return Document.Create(document =>
        {
            document.Page(page =>
            {
                page.Size(PageSizes.A4);
                page.Margin(30);
                page.DefaultTextStyle(x => x.FontSize(9));

                page.Header().Row(row =>
                {
                    row.RelativeItem().Column(column =>
                    {
                        column.Item().Text(nomeFantasia).FontSize(18).SemiBold();
                        column.Item().Text("Relatório de venda de produtos").FontSize(11).SemiBold();
                        column.Item().PaddingTop(5).Text(
                            $"Período: {FormatarData(dataInicial)} a {FormatarData(dataFinal)}");
                    });

                    if (logo is { Length: > 0 })
                        row.ConstantItem(50).Height(50).Image(logo);
                });

                page.Content().PaddingTop(15).Column(column =>
                {
                    column.Item().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            columns.RelativeColumn(4);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.5f);
                            columns.RelativeColumn(1.3f);
                            columns.RelativeColumn(1.7f);
                        });

                        table.Header(header =>
                        {
                            Cabecalho(header, "Produto");
                            Cabecalho(header, "Peso");
                            Cabecalho(header, "Tamanho");
                            Cabecalho(header, "Quantidade", alinharDireita: true);
                            Cabecalho(header, "Valor", alinharDireita: true);
                        });

                        foreach (var item in itens)
                        {
                            Celula(table, item.Descricao);
                            Celula(table, item.Peso ?? "-");
                            Celula(table, item.Tamanho ?? "-");
                            Celula(table, item.Quantidade.ToString("N2"), alinharDireita: true);
                            Celula(table, item.ValorTotal.FormatMoney(temSimboloDeDinheiro: true), alinharDireita: true);
                        }
                    });

                    column.Item().PaddingTop(15).AlignRight().Column(totais =>
                    {
                        totais.Item().Text($"Quantidade total: {quantidadeTotal:N2}").SemiBold();
                        totais.Item().Text($"Valor total: {valorTotal.FormatMoney(temSimboloDeDinheiro: true)}")
                            .FontSize(12).Bold();
                    });
                });

                page.Footer().AlignCenter().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
        }).GeneratePdf();
    }

    private static string FormatarData(DateTime? data) =>
        data?.DateTimeSomenteDataToString() ?? "Não informada";

    private static void Cabecalho(TableCellDescriptor table, string texto, bool alinharDireita = false)
    {
        var container = table.Cell()
            .Background(Colors.Grey.Darken2)
            .Padding(5);

        (alinharDireita ? container.AlignRight() : container.AlignLeft())
            .Text(texto)
            .FontColor(Colors.White)
            .SemiBold();
    }

    private static void Celula(TableDescriptor table, string texto, bool alinharDireita = false)
    {
        var container = table.Cell()
            .BorderBottom(0.5f)
            .BorderColor(Colors.Grey.Lighten2)
            .Padding(5);

        (alinharDireita ? container.AlignRight() : container.AlignLeft()).Text(texto);
    }
}
