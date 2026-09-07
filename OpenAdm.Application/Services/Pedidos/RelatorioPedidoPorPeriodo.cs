using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;
using OpenAdm.Pdf.DTOs;
using OpenAdm.Pdf.Interfaces;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class RelatorioPedidoPorPeriodo : IRelatorioPedidoPorPeriodo
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    public RelatorioPedidoPorPeriodo(
        IPedidoRepository pedidoRepository,
        IPdfPedidoService pdfPedidoService,
        IParceiroAutenticado parceiroAutenticado)
    {
        _pedidoRepository = pedidoRepository;
        _pdfPedidoService = pdfPedidoService;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<(byte[] pdf, int count)> GetRelatorioAsync(RelatorioPedidoDto relatorioPedidoDto)
    {
        relatorioPedidoDto.Validar();

        var pedidos = await _pedidoRepository
            .GetPedidosByRelatorioPorPeriodoAsync(relatorioPedidoDto);

        var parceiro = await _parceiroAutenticado.ObterParceiroAutenticadoAsync();

        var total = pedidos.Sum(x => x.ValorTotal);
        var relatorioPedidoModel = new GerarRelatorioPedidoDTO(
            relatorioPedidoDto.DataInicial,
            relatorioPedidoDto.DataFinal,
            parceiro.Logo,
            total);

        relatorioPedidoModel.RelatorioItensPedidoDto = pedidos.Select(pedido =>
        {
            var quantidade = pedido.ItensPedido.Sum(x => x.Quantidade);
            return new RelatorioItensPedidoDto(
                pedido.Numero,
                pedido.Usuario.Nome,
                quantidade,
                pedido.ValorTotal,
                pedido.DataDeCriacao);
        }).ToList();

        var pdf = _pdfPedidoService.GeneratePdfPedidoRelatorio(relatorioPedidoModel, parceiro.NomeFantasia, pedidos);

        return (pdf, pedidos.Count);
    }

    public async Task<RelatorioPedidoListagemDto> GetListagemAsync(RelatorioPedidoDto relatorioPedidoDto)
    {
        relatorioPedidoDto.Validar();

        var pedidos = await _pedidoRepository
            .GetPedidosByRelatorioPorPeriodoAsync(relatorioPedidoDto);

        var totais = new RelatorioPedidoTotaisDto(
            pedidos.Count,
            pedidos.Sum(x => x.ItensPedido.Sum(item => item.Quantidade)),
            pedidos.Sum(x => x.ValorTotal));

        var totaisPorUsuario = pedidos
            .GroupBy(x => new { x.UsuarioId, x.Usuario.Nome })
            .Select(grupo =>
            {
                var produtos = grupo
                    .SelectMany(x => x.ItensPedido)
                    .GroupBy(x => new { x.ProdutoId, x.Produto.Descricao })
                    .Select(produto => new RelatorioPedidoProdutoDto(
                        produto.Key.ProdutoId,
                        produto.Key.Descricao,
                        produto.Sum(x => x.Quantidade),
                        produto.Sum(x => x.ValorTotal)))
                    .ToList();

                var topProdutosPorQuantidade = produtos
                    .OrderByDescending(x => x.Quantidade)
                    .ThenByDescending(x => x.ValorTotal)
                    .ThenBy(x => x.Produto)
                    .Take(3)
                    .ToList();

                var topProdutosPorValor = produtos
                    .OrderByDescending(x => x.ValorTotal)
                    .ThenByDescending(x => x.Quantidade)
                    .ThenBy(x => x.Produto)
                    .Take(3)
                    .ToList();

                return new RelatorioPedidoTotaisUsuarioDto(
                    grupo.Key.UsuarioId,
                    grupo.Key.Nome,
                    grupo.Count(),
                    grupo.Sum(x => x.ItensPedido.Sum(item => item.Quantidade)),
                    grupo.Sum(x => x.ValorTotal),
                    topProdutosPorQuantidade,
                    topProdutosPorValor);
            })
            .OrderByDescending(x => x.ValorTotal)
            .ToList();

        var values = pedidos
            .Select(pedido => new RelatorioPedidoItemDto(
                pedido.Id,
                pedido.Numero,
                pedido.UsuarioId,
                pedido.Usuario.Nome,
                pedido.ItensPedido.Sum(x => x.Quantidade),
                pedido.ValorTotal,
                pedido.DataDeCriacao))
            .ToList();

        return new RelatorioPedidoListagemDto(
            values,
            totais,
            totaisPorUsuario);
    }
}
