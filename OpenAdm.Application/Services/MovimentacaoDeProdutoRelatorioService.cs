using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;
using OpenAdm.Domain.Extensions;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OpenAdm.Application.Services;

public sealed class MovimentacaoDeProdutoRelatorioService : IMovimentacaoDeProdutoRelatorioService
{
    private readonly IList<string> _colunsName = new List<string>()
    {
        "Data",
        "REF",
        "Descrição",
        "Categoria",
        "Peso/Tamanho",
        "Tipo",
        "Quantidade"
    };
    private static readonly IList<int> _colunsWidt = new List<int>()
    {
        80,50,140,80,60,70,60
    };
    public byte[] ObterPdfAsync(
        IList<MovimentacaoDeProdutoRelatorio> movimentacaoDeProdutoRelatorios,
        string nomeFantasia,
        DateTime dataInicial,
        DateTime dataFinal,
        string? logo)
    {
        void HeaderCustom(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(18).SemiBold();
            var titleStyle2 = TextStyle.Default.FontSize(10).SemiBold();

            container.Row(row =>
            {
                row.RelativeItem(0.8f).Column(column =>
                {
                    column.Item().Text($"#{nomeFantasia}").Style(titleStyle);
                    column.Item().Text($"Relatório estoque por período").Style(titleStyle2);

                    column.Item().Text(text =>
                    {
                        text.Span("Data inicial: ").SemiBold().FontSize(10);
                        text.Span(dataInicial.DateTimeSomenteDataToString());
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Data final: ").SemiBold().FontSize(10);
                        text.Span(dataFinal.DateTimeSomenteDataToString());
                    });
                });

                if (!string.IsNullOrWhiteSpace(logo))
                {
                    row.ConstantItem(50).Height(50).Width(50).Image(Convert.FromBase64String(logo));
                }

            });
        }

        static IContainer CellStyleHeaderTable(IContainer container)
        {
            return container
                .DefaultTextStyle(x => x.SemiBold())
                .PaddingVertical(5)
                .BorderBottom(1)
                .BorderColor(Colors.Black);
        }

        static IContainer CellTableStyle(IContainer container)
        {
            return container
                .BorderBottom(1)
                .BorderColor(Colors.Grey.Lighten2)
                .PaddingVertical(5);
        }

        var pdf = Document
            .Create(container =>
            {
                container.Page(page =>
                {
                    page.Configurar();
                    page.Header().Element(HeaderCustom);
                    page.Content().Table(table =>
                    {
                        table.ColumnsDefinition(columns =>
                        {
                            foreach (var columnsWidth in _colunsWidt)
                            {
                                columns.ConstantColumn(columnsWidth);
                            }
                        });

                        table.Header(header =>
                        {
                            foreach (var columnsName in _colunsName)
                            {
                                header
                                    .Cell()
                                    .Element(CellStyleHeaderTable)
                                    .Text(columnsName)
                                    .FontSize(10);
                            }
                        });

                        foreach (var item in movimentacaoDeProdutoRelatorios)
                        {
                            table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text($"{item.DataDaMovimentacao.DateTimeToString()}")
                            .FontSize(8);

                            table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text($"#{item.Referencia ?? ""}")
                            .FontSize(8);

                            table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text(item.Descricao)
                            .FontSize(8);

                            table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text(item.Categoria)
                            .FontSize(8);

                            table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text(item.PesoTamanho)
                            .FontSize(8);

                            table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text(item.TipoMovimento)
                            .FontSize(8);

                            table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text(item.Quantidade.ConverterPadraoBrasileiro())
                            .FontSize(8);
                        }
                    });
                    page.FooterCustom();
                });
            }).GeneratePdf();

        return pdf;
    }
}
