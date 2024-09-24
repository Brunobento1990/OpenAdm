using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;
using System.Text;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class RelatorioPedidoPorPeriodo : IRelatorioPedidoPorPeriodo
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IPdfPedidoService _pdfPedidoService;
    public RelatorioPedidoPorPeriodo(
        IPedidoRepository pedidoRepository,
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IPdfPedidoService pdfPedidoService)
    {
        _pedidoRepository = pedidoRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _pdfPedidoService = pdfPedidoService;
    }

    public async Task<(byte[] pdf, int count)> GetRelatorioAsync(RelatorioPedidoDto relatorioPedidoDto)
    {
        var pedidos = await _pedidoRepository
            .GetPedidosByRelatorioPorPeriodoAsync(relatorioPedidoDto);

        var configuracaoDePedido = await _configuracoesDePedidoRepository
            .GetConfiguracoesDePedidoAsync();

        var logo = configuracaoDePedido?.Logo is null ? null : Encoding.UTF8.GetString(configuracaoDePedido.Logo);
        var total = pedidos.Sum(x => x.ValorTotal);
        var relatorioPedidoModel = new GerarRelatorioPedidoDto(
            relatorioPedidoDto.DataInicial,
            relatorioPedidoDto.DataFinal,
            logo,
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

        var pdf = _pdfPedidoService.GeneratePdfPedidoRelatorio(relatorioPedidoModel);

        return (pdf, pedidos.Count);
    }
}
