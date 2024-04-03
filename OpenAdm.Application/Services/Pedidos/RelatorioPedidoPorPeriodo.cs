using Domain.Pkg.Pdfs.Models;
using Domain.Pkg.Pdfs.Services;
using OpenAdm.Application.Interfaces.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model.Pedidos;
using System.Text;

namespace OpenAdm.Application.Services.Pedidos;

public sealed class RelatorioPedidoPorPeriodo : IRelatorioPedidoPorPeriodo
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;

    public RelatorioPedidoPorPeriodo(
        IPedidoRepository pedidoRepository,
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
    }

    public async Task<(byte[] pdf, int count)> GetRelatorioAsync(RelatorioPedidoDto relatorioPedidoDto)
    {
        var pedidos = await _pedidoRepository
            .GetPedidosByRelatorioPorPeriodoAsync(relatorioPedidoDto);

        var configuracaoDePedido = await _configuracoesDePedidoRepository
            .GetConfiguracoesDePedidoAsync();

        var logo = configuracaoDePedido?.Logo is null ? null : Encoding.UTF8.GetString(configuracaoDePedido.Logo);
        var total = pedidos.Sum(x => x.ValorTotal);
        var relatorioPedidoModel = new RelatorioPedidoModel(
            relatorioPedidoDto.DataInicial,
            relatorioPedidoDto.DataFinal,
            logo,
            total);

        relatorioPedidoModel.RelatorioItensPedidoModel = pedidos.Select(pedido =>
        {
            var quantidade = pedido.ItensPedido.Sum(x => x.Quantidade);
            return new RelatorioItensPedidoModel(
                pedido.Numero,
                pedido.Usuario.Nome,
                quantidade,
                pedido.ValorTotal,
                pedido.DataDeCriacao);
        }).ToList();

        var pdf = RelatorioPedidoService.GeneratePdf(relatorioPedidoModel);

        return (pdf, pedidos.Count);
    }
}
