﻿using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using OpenAdm.Domain.Extensions;
using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Pedidos;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class PdfPedidoService : IPdfPedidoService
{
    private readonly IList<string> _colunsName = new List<string>()
    {
        "REF",
        "Descrição",
        "Tamanho/Peso",
        "Quantidade",
        "Valor unitário",
        "Total"
    };
    private readonly IList<string> _colunsNameProducao = new List<string>()
    {
        "REF",
        "Descrição",
        "Categoria",
        "Tamanho",
        "Peso",
        "Quantidade"
    };
    private readonly IList<string> _colunsNamePedidoRelatorio = new List<string>()
    {
        "N.",
        "Data",
        "Cliente",
        "Quantidade itens",
        "Total"
    };
    private static readonly IList<int> _colunsWidt = new List<int>()
    {
        60,150,80,70,90,50
    };
    private static readonly IList<int> _colunsWidtProducao = new List<int>()
    {
        60,150,80,70,90,50
    };
    private static readonly IList<int> _colunsWidtRelatorio = new List<int>()
    {
        70,90,170,90,90
    };


    public byte[] GeneratePdfPedido(
        Pedido pedido,
        Parceiro parceiro)
    {
        var titleStyle = TextStyle.Default.FontSize(18).SemiBold();
        var titleStyle2 = TextStyle.Default.FontSize(10).SemiBold();
        var titleStyleName = TextStyle.Default.FontSize(10);

        void HeaderCustom(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"#{parceiro.NomeFantasia}").Style(titleStyle);

                    column.Item().Text(text =>
                    {
                        text.Span("Data de emissão: ").SemiBold().FontSize(14);
                        text.Span(pedido.DataDeCriacao.DateTimeToString());
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Pedido número: ").Style(titleStyle2);
                        text.Span(pedido.Numero.ToString()).Style(titleStyleName);
                    });

                    column.Item().PaddingTop(10).Text(text =>
                    {
                        text.Span("Cliente: ").Style(titleStyle2);
                        text.Span(pedido.Usuario.Nome).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Email: ").Style(titleStyle2);
                        text.Span(pedido.Usuario.Email).Style(titleStyleName);
                    });

                    column.Item().Text(text =>
                    {
                        text.Span("Telefone: ").Style(titleStyle2);
                        text.Span(string.Format("{0:(00)00000-0000}", pedido.Usuario.Telefone)).Style(titleStyleName);
                    });

                    if (!string.IsNullOrWhiteSpace(pedido.Usuario.Cnpj))
                    {
                        column.Item().Text(text =>
                        {
                            text.Span("CNPJ: ").Style(titleStyle2);
                            text.Span(pedido.Usuario.Cnpj.FormatCnpj()).Style(titleStyleName);
                        });
                    }
                    if (!string.IsNullOrWhiteSpace(pedido.Usuario.Cpf))
                    {
                        column.Item().Text(text =>
                        {
                            text.Span("CPF: ").Style(titleStyle2);
                            text.Span(pedido.Usuario.Cpf.FormatCpf()).Style(titleStyleName);
                        });
                    }
                });

                if (parceiro.Logo != null)
                {
                    row.ConstantItem(50).Height(50).Width(50).Image(Convert.FromBase64String(parceiro.Logo.FromBytes()));
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

                        pedido.ItensPedido = [.. pedido.ItensPedido.OrderBy(x => x.Numero)];
                        var itemsPedidosGroup = pedido.ItensPedido.GroupBy(x => x.ProdutoId);

                        foreach (var itensGroup in itemsPedidosGroup)
                        {
                            foreach (var item in itensGroup)
                            {
                                table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.Produto.Referencia)
                                .FontSize(8);

                                table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.Produto.Descricao)
                                .FontSize(8);

                                table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.Tamanho?.Descricao ?? item.Peso?.Descricao)
                                .FontSize(8);

                                table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.Quantidade.FormatMoney())
                                .FontSize(8);

                                table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.ValorUnitario.FormatMoney())
                                .FontSize(8);

                                table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text(item.ValorTotal.FormatMoney())
                                .FontSize(8);
                            }
                        }

                        table.Cell();
                        table.Cell();
                        table.Cell();
                        table.Cell();
                        table.Cell();

                        table
                            .Cell()
                            .Element(CellTableStyle)
                            .Text($"Total : {pedido.ValorTotal.FormatMoney()}")
                            .FontSize(8);


                        var tamamhosItens = pedido
                            .ItensPedido
                            .OrderBy(x => x.Tamanho?.Numero)
                            .Where(x => x.Tamanho != null)
                            .Select(x => x.Tamanho)
                            .ToList()
                            .GroupBy(x => x?.Id);

                        var pesosItens = pedido
                            .ItensPedido
                            .OrderBy(x => x.Peso?.Numero)
                            .Where(x => x.Peso != null)
                            .Select(x => x.Peso)
                            .ToList()
                            .GroupBy(x => x?.Id);


                        if (tamamhosItens.Count() > 0)
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
                            table.Cell();
                            var itemPedido = pedido
                                .ItensPedido
                                .FirstOrDefault(x => x.TamanhoId == tamanhoGroup.Key);

                            var totalQuantidade = pedido
                                .ItensPedido
                                .Where(x => x.TamanhoId == tamanhoGroup.Key)
                                .ToList()
                                .Sum(x => x.Quantidade);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text($"{itemPedido?.Tamanho?.Descricao} : {totalQuantidade.FormatMoney()}")
                                .FontSize(8);
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();

                            count++;
                        }

                        if (pesosItens.Count() > 0)
                        {
                            table
                            .Cell()
                            .Element(CellStyleHeaderTable)
                            .Text($"Pesos")
                            .FontSize(10);
                        }

                        count = 0;

                        foreach (var pedoGroup in pesosItens)
                        {
                            if (count > 0)
                            {
                                table.Cell();
                            }

                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            var itemPedido = pedido
                                .ItensPedido
                                .FirstOrDefault(x => x.PesoId == pedoGroup.Key);

                            var totalQuantidade = pedido
                                .ItensPedido
                                .Where(x => x.PesoId == pedoGroup.Key)
                                .ToList()
                                .Sum(x => x.Quantidade);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text($"{itemPedido?.Peso?.Descricao} : {totalQuantidade.FormatMoney()}")
                                .FontSize(8);

                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();

                            count++;
                        }
                    });
                    page.FooterCustom();
                });

                if (pedido.EnderecoEntrega != null)
                {
                    container.Page(page =>
                    {
                        page.Margin(30);
                        page.Content().Column(col =>
                        {
                            if (parceiro.EnderecoParceiro != null)
                            {
                                col.Item().PaddingTop(25).Element(container =>
                                {
                                    container.Border(1)
                                        .BorderColor(Colors.Black)
                                        .Padding(10)
                                        .Column(column =>
                                        {
                                            column.Item().Text(text =>
                                            {
                                                text.Span("Remetente: ").Style(titleStyle2);
                                                text.Span(parceiro.NomeFantasia).Style(titleStyleName);
                                            });

                                            column.Item().Text(text =>
                                            {
                                                text.Span("Rua: ").Style(titleStyle2);
                                                text.Span(parceiro.EnderecoParceiro.Logradouro).Style(titleStyleName);
                                            });

                                            column.Item().Text(text =>
                                            {
                                                text.Span("Número: ").Style(titleStyle2);
                                                text.Span(parceiro.EnderecoParceiro.Numero).Style(titleStyleName);
                                            });

                                            column.Item().Text(text =>
                                            {
                                                text.Span("Cidade: ").Style(titleStyle2);
                                                text.Span($"{parceiro.EnderecoParceiro.Localidade} - {pedido.EnderecoEntrega.Uf}").Style(titleStyleName);
                                            });

                                            column.Item().Text(text =>
                                            {
                                                text.Span("Bairro: ").Style(titleStyle2);
                                                text.Span($"{parceiro.EnderecoParceiro.Bairro}").Style(titleStyleName);
                                            });

                                            column.Item().Text(text =>
                                            {
                                                text.Span("CEP: ").Style(titleStyle2);
                                                text.Span($"{parceiro.EnderecoParceiro.Cep}").Style(titleStyleName);
                                            });

                                            column.Item().Text(text =>
                                            {
                                                text.Span("Complemento: ").Style(titleStyle2);
                                                text.Span(parceiro.EnderecoParceiro.Complemento ?? "").Style(titleStyleName);
                                            });
                                        });
                                });
                            }

                            col.Item().PaddingTop(25).Element(container =>
                            {
                                container.Border(1)
                                    .BorderColor(Colors.Black)
                                    .Padding(10)
                                    .Column(column =>
                                    {
                                        column.Item().Text(text =>
                                        {
                                            text.Span("Pedido: ").Style(titleStyle2);
                                            text.Span($"{pedido.Numero}").Style(titleStyleName);
                                        });

                                        column.Item().Text(text =>
                                        {
                                            text.Span("Destinatário: ").Style(titleStyle2);
                                            text.Span(pedido.Usuario.Nome).Style(titleStyleName);
                                        });

                                        column.Item().Text(text =>
                                        {
                                            text.Span("Rua: ").Style(titleStyle2);
                                            text.Span(pedido.EnderecoEntrega.Logradouro).Style(titleStyleName);
                                        });

                                        column.Item().Text(text =>
                                        {
                                            text.Span("Número: ").Style(titleStyle2);
                                            text.Span(pedido.EnderecoEntrega.Numero).Style(titleStyleName);
                                        });

                                        column.Item().Text(text =>
                                        {
                                            text.Span("Cidade: ").Style(titleStyle2);
                                            text.Span($"{pedido.EnderecoEntrega.Localidade} - {pedido.EnderecoEntrega.Uf}").Style(titleStyleName);
                                        });

                                        column.Item().Text(text =>
                                        {
                                            text.Span("Bairro: ").Style(titleStyle2);
                                            text.Span($"{pedido.EnderecoEntrega.Bairro}").Style(titleStyleName);
                                        });

                                        column.Item().Text(text =>
                                        {
                                            text.Span("CEP: ").Style(titleStyle2);
                                            text.Span($"{pedido.EnderecoEntrega.Cep}").Style(titleStyleName);
                                        });

                                        column.Item().Text(text =>
                                        {
                                            text.Span("Complemento: ").Style(titleStyle2);
                                            text.Span(pedido.EnderecoEntrega.Complemento ?? "").Style(titleStyleName);
                                        });
                                    });
                            });
                        });
                    });
                }

            }).GeneratePdf();

        return pdf;
    }

    public byte[] GeneratePdfPedidoRelatorio(
        GerarRelatorioPedidoDto relatorioPedidoDto,
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
                            .OrderBy(x => x.Tamanho?.Numero)
                            .Where(x => x.Tamanho != null)
                            .Select(x => x.Tamanho)
                            .ToList()
                            .GroupBy(x => x?.Id);

                        var pesosItens = itensPedido
                            .OrderBy(x => x.Peso?.Numero)
                            .Where(x => x.Peso != null)
                            .Select(x => x.Peso)
                            .ToList()
                            .GroupBy(x => x?.Id);


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
                            //table.Cell();
                            var itemPedido = itensPedido
                                .FirstOrDefault(x => x.TamanhoId == tamanhoGroup.Key);

                            var totalQuantidade = itensPedido
                                .Where(x => x.TamanhoId == tamanhoGroup.Key)
                                .ToList()
                                .Sum(x => x.Quantidade);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text($"{itemPedido?.Tamanho?.Descricao} : {totalQuantidade.FormatMoney()}")
                                .FontSize(8);
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            //table.Cell();

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

                        foreach (var pedoGroup in pesosItens)
                        {
                            if (count > 0)
                            {
                                table.Cell();
                            }

                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            //table.Cell();
                            var itemPedido = itensPedido
                                .FirstOrDefault(x => x.PesoId == pedoGroup.Key);

                            var totalQuantidade = itensPedido
                                .Where(x => x.PesoId == pedoGroup.Key)
                                .ToList()
                                .Sum(x => x.Quantidade);

                            table
                                .Cell()
                                .Element(CellTableStyle)
                                .Text($"{itemPedido?.Peso?.Descricao} : {totalQuantidade.FormatMoney()}")
                                .FontSize(8);

                            table.Cell();
                            table.Cell();
                            table.Cell();
                            table.Cell();
                            //table.Cell();

                            count++;
                        }
                    });
                    page.FooterCustom();
                });
            }).GeneratePdf();

        return pdf;
    }

    public byte[] ProducaoPedido(
        IList<ItemPedidoProducaoViewModel> itemPedidoProducaoViewModels,
        string nomeFantasia,
        string? logo,
        IList<string> pedidos)
    {
        void HeaderCustom(IContainer container)
        {
            var titleStyle = TextStyle.Default.FontSize(18).SemiBold();
            var titleStyle2 = TextStyle.Default.FontSize(10).SemiBold();
            var titleStyleName = TextStyle.Default.FontSize(10);

            container.Row(row =>
            {
                row.RelativeItem().Column(column =>
                {
                    column.Item().Text($"#{nomeFantasia}").Style(titleStyle);
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
