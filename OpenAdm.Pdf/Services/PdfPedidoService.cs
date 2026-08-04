using System.Globalization;
using System.Text.RegularExpressions;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Extensions;
using OpenAdm.Pdf.DTOs;
using OpenAdm.Pdf.Interfaces;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace OpenAdm.Pdf.Services;

internal class PdfPedidoService : IPdfPedidoService
{
    private static readonly IList<string> _colunsNameProducao = new List<string>()
    {
        "REF",
        "Descrição",
        "Categoria",
        "Tamanho",
        "Peso",
        "Qtd"
    };

    private static readonly IList<string> _colunsNamePedidoRelatorio = new List<string>()
    {
        "N.",
        "Data",
        "Cliente",
        "Quantidade itens",
        "Total"
    };

    private static readonly IList<int> _colunsWidtProducao = new List<int>()
    {
        60, 150, 80, 70, 90, 50
    };

    private static readonly IList<int> _colunsWidtRelatorio = new List<int>()
    {
        70, 90, 170, 90, 90
    };


    public byte[] GeneratePdfPedido(
        Pedido pedido,
        Parceiro parceiro)
    {
        var pdf = Document
            .Create(container =>
            {
                container.Page(page =>
                {
                    page.Configurar();
                    page.Margin(12, Unit.Millimetre);
                    page.Header().Element(header => ComposePedidoHeader(header, pedido, parceiro));
                    page.Content().PaddingTop(14).Column(column =>
                    {
                        column.Spacing(14);
                        column.Item().Element(container => ComposeClienteSection(container, pedido.Usuario));
                        column.Item().Element(container => ComposeComplementoPedidoSection(container, pedido));
                        column.Item().Element(container => ComposeItensTable(container, pedido));
                        column.Item().Element(container => ComposeResumoPedidoSection(container, pedido));
                    });
                    page.Footer().Element(container => ComposePedidoFooter(container, pedido));
                });

                if (pedido.EnderecoEntrega != null)
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(12, Unit.Millimetre);
                        page.PageColor(Colors.White);
                        page.DefaultTextStyle(x => x.FontSize(10));
                        page.Content().Element(container => ComposeEtiqueta(container, pedido, parceiro));
                        page.Footer().Element(container => ComposePedidoFooter(container, pedido));
                    });
                }
            }).GeneratePdf();

        return pdf;
    }

    private static void ComposePedidoHeader(IContainer container, Pedido pedido, Parceiro parceiro)
    {
        container.BorderBottom(1)
            .BorderColor(Colors.Grey.Lighten1)
            .PaddingBottom(10)
            .Row(row =>
            {
                row.RelativeItem(2).AlignMiddle().Column(column =>
                {
                    if (parceiro.Logo is { Length: > 0 })
                    {
                        column.Item().Width(118).Height(48).Image(parceiro.Logo).FitArea();
                    }
                    else
                    {
                        column.Item().Text(parceiro.NomeFantasia)
                            .FontSize(20)
                            .Bold()
                            .FontColor(Colors.Grey.Darken4);
                    }
                });

                row.RelativeItem(3).AlignMiddle().AlignCenter().Column(column =>
                {
                    column.Item().AlignCenter().Text("PEDIDO DE VENDA")
                        .FontSize(21)
                        .Bold()
                        .FontColor(Colors.Grey.Darken4);
                    column.Item().PaddingTop(5).AlignCenter().Text($"Pedido nº {pedido.Numero}")
                        .FontSize(16)
                        .Bold()
                        .FontColor(Colors.Blue.Darken3);
                });

                row.RelativeItem(2).AlignMiddle().AlignRight().Column(column =>
                {
                    column.Item().Text("Emissão").FontSize(9).SemiBold().FontColor(Colors.Grey.Darken2);
                    column.Item().PaddingTop(3).Text(pedido.DataDeCriacao.DateTimeToString()).FontSize(10).FontColor(Colors.Grey.Darken4);
                });
            });
    }

    private static void ComposeClienteSection(IContainer container, Usuario usuario)
    {
        container.Element(SectionBox).Column(column =>
        {
            column.Item().Element(container => SectionTitle(container, "DADOS DO CLIENTE"));
            column.Item().PaddingTop(10).Row(row =>
            {
                row.RelativeItem().Element(item => Field(item, "Nome/Razão social", usuario.Nome));
                row.RelativeItem().Element(item => Field(item, "CPF/CNPJ", string.IsNullOrWhiteSpace(usuario.Cnpj) ? usuario.Cpf?.FormatCpf() : usuario.Cnpj.FormatCnpj()));
            });
            column.Item().Row(row =>
            {
                row.RelativeItem().Element(item => Field(item, "Telefone", usuario.Telefone?.FormatPhone()));
                row.RelativeItem().Element(item => Field(item, "E-mail", usuario.Email));
            });
        });
    }

    private static void ComposeComplementoPedidoSection(IContainer container, Pedido pedido)
    {
        var endereco = pedido.EnderecoEntrega;
        var primeiraParcela = pedido.Fatura?.Parcelas.OrderBy(x => x.NumeroDaParcela).FirstOrDefault();

        if (endereco == null && primeiraParcela?.MeioDePagamento == null && string.IsNullOrWhiteSpace(primeiraParcela?.Observacao))
        {
            container.Height(0);
            return;
        }

        container.Element(SectionBox).Column(column =>
        {
            column.Item().Element(container => SectionTitle(container, "INFORMAÇÕES COMPLEMENTARES"));

            if (endereco != null)
            {
                column.Item().PaddingTop(4).Column(enderecoColumn =>
                {
                    enderecoColumn.Item().Row(row =>
                    {
                        row.Spacing(8);
                        row.RelativeItem(3).Element(item => Field(item, "Logradouro", $"{endereco.Logradouro} nº {endereco.Numero}"));
                    });

                    enderecoColumn.Item().Row(row =>
                    {
                        row.Spacing(8);
                        row.RelativeItem().Element(item => Field(item, "Bairro", endereco.Bairro));
                        row.RelativeItem().Element(item => Field(item, "Cidade/UF", $"{endereco.Localidade}/{endereco.Uf}"));
                        row.RelativeItem().Element(item => Field(item, "CEP", endereco.Cep));
                    });

                    if (!string.IsNullOrWhiteSpace(endereco.Complemento) || !string.IsNullOrWhiteSpace(endereco.TipoFrete))
                    {
                        enderecoColumn.Item().Row(row =>
                        {
                            row.Spacing(8);
                            row.RelativeItem().Element(item => Field(item, "Complemento", endereco.Complemento));
                            row.RelativeItem().Element(item => Field(item, "Prazo/Entrega", endereco.TipoFrete));
                        });
                    }
                });
            }

            if (primeiraParcela?.MeioDePagamento != null || !string.IsNullOrWhiteSpace(primeiraParcela?.Observacao))
            {
                column.Item().PaddingTop(endereco != null ? 2 : 10).Row(row =>
                {
                    row.Spacing(8);
                    row.RelativeItem().Element(item => Field(item, "Forma de pagamento", primeiraParcela?.MeioDePagamento?.ToString()));
                    row.RelativeItem(2).Element(item => Field(item, "Observações", primeiraParcela?.Observacao));
                });
            }
        });
    }

    private static void ComposeItensTable(IContainer container, Pedido pedido)
    {
        container.Table(table =>
        {
            table.ColumnsDefinition(columns =>
            {
                columns.ConstantColumn(42);
                columns.RelativeColumn(3.2f);
                columns.ConstantColumn(68);
                columns.ConstantColumn(52);
                columns.ConstantColumn(66);
                columns.ConstantColumn(66);
            });

            table.Header(header =>
            {
                HeaderCell(header, "Ref.", CellAlignment.Left);
                HeaderCell(header, "Produto", CellAlignment.Left);
                HeaderCell(header, "Tamanho/Peso", CellAlignment.Center);
                HeaderCell(header, "Qtd", CellAlignment.Right);
                HeaderCell(header, "Valor unitário", CellAlignment.Right);
                HeaderCell(header, "Subtotal", CellAlignment.Right);
            });

            var index = 0;

            foreach (var item in pedido.ItensPedido.OrderBy(x => x.Produto.Numero).ThenBy(x => x.Tamanho?.Descricao).ThenBy(x => x.Peso?.Descricao))
            {
                var alternate = index % 2 == 1;
                BodyCell(table, item.Produto.Referencia ?? "", CellAlignment.Left, alternate);
                BodyCell(table, string.IsNullOrWhiteSpace(item.Produto.Referencia) ?
                    item.Produto.Descricao :
                    item.Produto.Descricao.Replace(item.Produto.Referencia ?? "", "").Replace("-", "").Trim(), CellAlignment.Left, alternate);
                BodyCell(table, item.Tamanho?.Descricao ?? item.Peso?.Descricao ?? "", CellAlignment.Center, alternate);
                BodyCell(table, item.Quantidade.ToString(), CellAlignment.Right, alternate);
                BodyCell(table, item.ValorUnitario.FormatMoney(temSimboloDeDinheiro: true), CellAlignment.Right, alternate);
                BodyCell(table, item.ValorTotal.FormatMoney(temSimboloDeDinheiro: true), CellAlignment.Right, alternate);
                index++;
            }
        });
    }

    private static void ComposeResumoPedidoSection(IContainer container, Pedido pedido)
    {
        var exibirResumoQuantidade = HasResumoQuantidade(pedido);

        container.ShowEntire().Row(row =>
        {
            row.Spacing(14);

            if (exibirResumoQuantidade)
            {
                row.RelativeItem().Element(item => ComposeResumoQuantidade(item, pedido));
            }
            else
            {
                row.RelativeItem();
            }

            row.ConstantItem(250).Element(item => ComposeResumoFinanceiro(item, pedido));
        });
    }

    private static void ComposeResumoFinanceiro(IContainer container, Pedido pedido)
    {
        var frete = pedido.EnderecoEntrega?.ValorFrete ?? 0;

        container.ShowEntire().AlignRight().Width(250)
            .Border(1)
            .BorderColor(Colors.Grey.Lighten1)
            .Background(Colors.Grey.Lighten5)
            .Padding(12)
            .Column(column =>
        {
            column.Spacing(6);
            column.Item().Text("RESUMO FINANCEIRO").FontSize(12).Bold().FontColor(Colors.Grey.Darken4);
            FinancialLine(column, "Subtotal dos produtos", pedido.ValorTotal);

            if (frete > 0)
            {
                FinancialLine(column, "Frete", frete);
            }

            column.Item().PaddingTop(8).BorderTop(1).BorderColor(Colors.Grey.Lighten1).PaddingTop(8).Column(totalColumn =>
            {
                totalColumn.Item().AlignRight().Text("TOTAL DO PEDIDO").FontSize(11).Bold().FontColor(Colors.Grey.Darken4);
                totalColumn.Item().AlignRight().Text(pedido.ValorTotalCobrar.FormatMoney(temSimboloDeDinheiro: true))
                    .FontSize(19)
                    .Bold()
                    .FontColor(Colors.Blue.Darken3);
            });
        });
    }

    private static void ComposeResumoQuantidade(IContainer container, Pedido pedido)
    {
        var tamanhos = pedido.ItensPedido
            .Where(x => x.TamanhoId != null)
            .GroupBy(x => x.TamanhoId)
            .Select(g => new
            {
                Descricao = g.First().Tamanho!.Descricao,
                Quantidade = g.Sum(x => x.Quantidade),
                Valor = g.Sum(x => x.ValorTotal)
            })
            .ToList();

        var pesos = pedido.ItensPedido
            .Where(x => x.PesoId != null)
            .GroupBy(x => x.PesoId)
            .Select(g => new
            {
                Descricao = g.First().Peso!.Descricao,
                Quantidade = g.Sum(x => x.Quantidade),
                Valor = g.Sum(x => x.ValorTotal)
            })
            .ToList();

        if (!tamanhos.Any() && !pesos.Any())
        {
            container.Height(0);
            return;
        }

        container.Element(SectionBox).Column(column =>
        {
            if (tamanhos.Any())
            {
                column.Item().Element(container => SectionTitle(container, "RESUMO POR TAMANHO"));
                foreach (var tamanho in tamanhos)
                {
                    column.Item().PaddingTop(4).Text($"{tamanho.Descricao.ToLower()} - {tamanho.Quantidade} un - {tamanho.Valor.FormatMoney(temSimboloDeDinheiro: true)}").FontSize(10).FontColor(Colors.Grey.Darken3);
                }
            }

            if (pesos.Any())
            {
                column.Item().PaddingTop(tamanhos.Any() ? 8 : 0).Element(container => SectionTitle(container, "RESUMO POR PESO"));
                foreach (var peso in pesos)
                {
                    column.Item().PaddingTop(4).Text($"{peso.Descricao.ToLower()} - {peso.Quantidade} un - {peso.Valor.FormatMoney(temSimboloDeDinheiro: true)}").FontSize(10).FontColor(Colors.Grey.Darken3);
                }
            }
        });
    }

    private static bool HasResumoQuantidade(Pedido pedido)
    {
        return pedido.ItensPedido.Any(x => x.TamanhoId != null || x.PesoId != null);
    }

    private static void ComposePedidoFooter(IContainer container, Pedido pedido)
    {
        container.BorderTop(1)
            .BorderColor(Colors.Grey.Lighten2)
            .PaddingTop(5)
            .DefaultTextStyle(x => x.FontSize(7).FontColor(Colors.Grey.Darken1))
            .Row(row =>
            {
                row.RelativeItem().Text($"Pedido nº {pedido.Numero} - Impressão em {DateTime.Now.DateTimeToString()}");
                row.ConstantItem(80).AlignRight().Text(text =>
                {
                    text.Span("Página ");
                    text.CurrentPageNumber();
                    text.Span(" de ");
                    text.TotalPages();
                });
            });
    }

    private static void ComposeEtiqueta(IContainer container, Pedido pedido, Parceiro parceiro)
    {
        container.Column(column =>
        {
            column.Spacing(12);

            if (parceiro.EnderecoParceiro != null)
            {
                column.Item().Element(item => EtiquetaEnderecoBlock(
                    item,
                    "REMETENTE",
                    parceiro.NomeFantasia,
                    null,
                    parceiro.EnderecoParceiro,
                    parceiro.Telefones.FirstOrDefault()?.Telefone?.FormatPhone(),
                    destaque: false));
            }

            column.Item()
                .AlignCenter()
                //.Border(1)
                //.BorderColor(Colors.Grey.Darken2)
                .PaddingVertical(8)
                .Text($"PEDIDO Nº {pedido.Numero}")
                .FontSize(18)
                .Bold();

            column.Item().Element(item => EtiquetaEnderecoBlock(
                item,
                "DESTINATÁRIO",
                pedido.Usuario.Nome,
                string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj) ? pedido.Usuario.Cpf?.FormatCpf() : pedido.Usuario.Cnpj.FormatCnpj(),
                pedido.EnderecoEntrega!,
                pedido.Usuario.Telefone?.FormatPhone(),
                destaque: true));
        });
    }

    private static IContainer SectionBox(IContainer container)
    {
        return container.Border(1)
            .BorderColor(Colors.Grey.Lighten2)
            .Background(Colors.Grey.Lighten5)
            .Padding(12);
    }

    private static void SectionTitle(IContainer container, string title)
    {
        container.Text(title).FontSize(12).Bold().FontColor(Colors.Grey.Darken4);
    }

    private static void Field(IContainer container, string label, string? value)
    {
        container.PaddingBottom(6).Column(column =>
        {
            column.Item().Text(label).FontSize(8).Bold().FontColor(Colors.Grey.Darken2);
            column.Item().PaddingTop(2).Text(string.IsNullOrWhiteSpace(value) ? "-" : value).FontSize(10).FontColor(Colors.Grey.Darken4);
        });
    }

    private enum CellAlignment
    {
        Left,
        Center,
        Right
    }

    private static void HeaderCell(TableCellDescriptor table, string text, CellAlignment alignment)
    {
        ApplyAlignment(table.Cell()
            .Background(Colors.Grey.Darken1)
            .PaddingVertical(7)
            .PaddingHorizontal(4), alignment)
            .Text(text)
            .FontSize(8.5f)
            .Bold()
            .FontColor(Colors.White);
    }

    private static void BodyCell(TableDescriptor table, string text, CellAlignment alignment, bool alternate)
    {
        ApplyAlignment(table.Cell()
            .Background(alternate ? Colors.Grey.Lighten5 : Colors.White)
            .BorderBottom(0.5f)
            .BorderColor(Colors.Grey.Lighten3)
            .PaddingVertical(6)
            .PaddingHorizontal(4), alignment)
            .Text(text)
            .FontSize(8.5f)
            .FontColor(Colors.Grey.Darken4);
    }

    private static IContainer ApplyAlignment(IContainer container, CellAlignment alignment)
    {
        return alignment switch
        {
            CellAlignment.Center => container.AlignCenter(),
            CellAlignment.Right => container.AlignRight(),
            _ => container.AlignLeft()
        };
    }

    private static void FinancialLine(ColumnDescriptor column, string label, decimal value)
    {
        column.Item().Row(row =>
        {
            row.RelativeItem().Text(label).FontSize(10).FontColor(Colors.Grey.Darken3);
            row.ConstantItem(95).AlignRight().Text(value.FormatMoney(temSimboloDeDinheiro: true)).FontSize(10).FontColor(Colors.Grey.Darken4);
        });
    }

    private static void EtiquetaEnderecoBlock(
        IContainer container,
        string titulo,
        string nome,
        string? documento,
        BaseEndereco endereco,
        string? telefone,
        bool destaque)
    {
        var fontSize = destaque ? 14 : 10;
        var labelSize = destaque ? 12 : 9;

        container.Border(1)
            .BorderColor(destaque ? Colors.Grey.Darken3 : Colors.Grey.Lighten1)
            .Padding(destaque ? 14 : 10)
            .Column(column =>
            {
                column.Item().Text(titulo).FontSize(labelSize).Bold().FontColor(Colors.Grey.Darken2);
                column.Item().PaddingTop(8).Text(nome).FontSize(fontSize + 2).Bold();

                if (!string.IsNullOrWhiteSpace(documento))
                {
                    column.Item().Text(documento).FontSize(fontSize);
                }

                column.Item().PaddingTop(6).Text($"{endereco.Logradouro}, {endereco.Numero}").FontSize(fontSize);
                column.Item().Text($"{endereco.Bairro} - {endereco.Localidade}/{endereco.Uf}").FontSize(fontSize);
                column.Item().Text($"CEP {endereco.Cep}").FontSize(fontSize);

                if (!string.IsNullOrWhiteSpace(endereco.Complemento))
                {
                    column.Item().Text(endereco.Complemento).FontSize(fontSize);
                }

                if (!string.IsNullOrWhiteSpace(telefone))
                {
                    column.Item().PaddingTop(6).Text($"Telefone {telefone}").FontSize(fontSize);
                }
            });
    }

    private static string FormatEndereco(BaseEndereco endereco)
    {
        var complemento = string.IsNullOrWhiteSpace(endereco.Complemento) ? "" : $" - {endereco.Complemento}";
        return $"{endereco.Logradouro}, {endereco.Numero} - {endereco.Bairro} - {endereco.Localidade}/{endereco.Uf} - CEP {endereco.Cep}{complemento}";
    }

    private static string OnlyDigits(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? ""
            : Regex.Replace(value, "\\D", "");
    }

    public byte[] GeneratePdfPedidoRelatorio(
        GerarRelatorioPedidoDTO relatorioPedidoDto,
        string nomeFantasia, IList<Pedido> pedidos)
    {
        var itensPedido = pedidos.SelectMany(x => x.ItensPedido);

        void HeaderCustom(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(18).SemiBold();
            var titleStyle2 = TextStyle.Default.FontSize(10).SemiBold();

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"#{nomeFantasia}").Style(titleStyle);
                    column.Item().Text($"Relatório de pedidos por período").Style(titleStyle2);

                    column.Item().Text(text =>
                    {
                        text.Span("Data de inicial: ").SemiBold().FontSize(10);
                        text.Span(relatorioPedidoDto.DataInicial.DateTimeToString());
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Data de final: ").SemiBold().FontSize(10);
                        text.Span(relatorioPedidoDto.DataFinal.DateTimeToString());
                    });
                });

                if (!string.IsNullOrWhiteSpace(relatorioPedidoDto.Logo))
                {
                    row.ConstantItem(50).Width(50).Height(50).Image(Convert.FromBase64String(relatorioPedidoDto.Logo));
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
                            foreach (var columnsWidth in _colunsWidtRelatorio)
                            {
                                columns.ConstantColumn(columnsWidth);
                            }
                        });

                        table.Header(header =>
                        {
                            foreach (var columnsName in _colunsNamePedidoRelatorio)
                            {
                                header
                                    .Cell()
                                    .Element(CellStyleHeaderTable)
                                    .Text(columnsName)
                                    .FontSize(10);
                            }
                        });

                        foreach (var item in relatorioPedidoDto.RelatorioItensPedidoDto)
                        {
                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text($"#{item.Numero}")
                                .FontSize(8);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.DataDeCadastro.DateTimeToString())
                                .FontSize(8);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.Cliente)
                                .FontSize(8);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.Quantidade.FormatMoney())
                                .FontSize(8);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.Total.FormatMoney())
                                .FontSize(8);
                        }

                        table.Cell();
                        table.Cell();
                        table.Cell();
                        table.Cell();

                        table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text($"Total : {relatorioPedidoDto.Total.FormatMoney()}")
                            .FontSize(8);

                        var tamamhosItens = itensPedido
                            .Where(x => x.TamanhoId != null)
                            .GroupBy(x => x.TamanhoId)
                            .Select(g => new
                            {
                                Descricao = g.First().Tamanho!.Descricao,
                                Numero = g.First().Tamanho!.Numero,
                                Total = g.Sum(x => x.Quantidade)
                            })
                            .OrderBy(x => x.Numero)
                            .ToList();

                        var pesosItens = itensPedido
                            .Where(x => x.PesoId != null)
                            .GroupBy(x => x.PesoId)
                            .Select(g => new
                            {
                                Descricao = g.First().Peso!.Descricao,
                                Numero = g.First().Peso!.Numero,
                                Total = g.Sum(x => x.Quantidade)
                            })
                            .OrderBy(x => x.Numero)
                            .ToList();


                        if (tamamhosItens.Any())
                        {
                            table
                                .Cell()
                                .Element(CellStyleHeaderTable)
                                .Text($"Tamanhos")
                                .FontSize(10);
                        }

                        var count = 0;

                        foreach (var tamanhoGroup in tamamhosItens)
                        {
                            if (count > 0)
                            {
                                table.Cell();
                            }

                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text($"{tamanhoGroup.Descricao} : {tamanhoGroup.Total.FormatMoney()}")
                                .FontSize(8);
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();

                            count++;
                        }

                        if (pesosItens.Any())
                        {
                            table
                                .Cell()
                                .Element(CellStyleHeaderTable)
                                .Text($"Pesos")
                                .FontSize(10);
                        }

                        count = 0;

                        foreach (var pesoGroup in pesosItens)
                        {
                            if (count > 0)
                            {
                                table.Cell();
                            }

                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text($"{pesoGroup.Descricao} : {pesoGroup.Total.FormatMoney()}")
                                .FontSize(8);

                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();

                            count++;
                        }
                    });
                    page.FooterCustom();
                });
            }).GeneratePdf();

        return pdf;
    }

    public byte[] ProducaoPedido(
        IList<ItemPedidoProducaoDTO> itemPedidoProducaoViewModels,
        string nomeFantasia,
        string? logo,
        IList<string> pedidos)
    {
        void HeaderCustom(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(18).SemiBold();

            container.Row(row =>
            {
                row.RelativeItem().Column(column => { column.Item().Text($"#{nomeFantasia}").Style(titleStyle); });

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
                    page.Content().Column(column =>
                    {
                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                foreach (var columnsWidth in _colunsWidtProducao)
                                {
                                    columns.ConstantColumn(columnsWidth);
                                }
                            });

                            table.Header(header =>
                            {
                                foreach (var columnsName in _colunsNameProducao)
                                {
                                    header
                                        .Cell()
                                        .Element(CellStyleHeaderTable)
                                        .Text(columnsName)
                                        .FontSize(10);
                                }
                            });

                            foreach (var item in itemPedidoProducaoViewModels)
                            {
                                table.Cell().Element(CellTableStyle).Text(item.Referencia).FontSize(8);
                                table.Cell().Element(CellTableStyle).Text(item.Produto).FontSize(8);
                                table.Cell().Element(CellTableStyle).Text(item.Categoria).FontSize(8);
                                table.Cell().Element(CellTableStyle).Text(item.Tamanho ?? "").FontSize(8);
                                table.Cell().Element(CellTableStyle).Text(item.Peso ?? "").FontSize(8);
                                table.Cell().Element(CellTableStyle).Text(item.Quantidade.FormatMoney()).FontSize(8);
                            }
                        });

                        if (pedidos.Count > 0)
                        {
                            column.Item().Table(table =>
                            {
                                table.ColumnsDefinition(columns =>
                                {
                                    columns.RelativeColumn();
                                    columns.RelativeColumn();
                                });

                                table.Cell().Element(CellTableStyle).Text("Pedidos").Bold().FontSize(8);
                                table.Cell(); // Preencher com célula vazia

                                foreach (var pedido in pedidos)
                                {
                                    table.Cell().Element(CellTableStyle).Text($"{pedido}")
                                        .FontSize(8);
                                    table.Cell();
                                }
                            });
                        }
                    });
                    page.FooterCustom();
                });
            }).GeneratePdf();

        return pdf;
    }
}
