using Domain.Pkg.Entities;
using Domain.Pkg.Enum;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using Domain.Pkg.Pdfs.Services;
using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;
using System.Reactive.Linq;

namespace OpenAdm.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepository,
    ITabelaDePrecoRepository tabelaDePrecoRepository,
    IProcessarPedidoService processarPedidoService)
    : IPedidoService
{
    private readonly ITabelaDePrecoRepository _tabelaDePrecoRepository = tabelaDePrecoRepository;
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
    private readonly IProcessarPedidoService _processarPedidoService = processarPedidoService;

    public async Task<PedidoViewModel> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto, Guid clienteId)
    {
        var tabelaDePreco = await _tabelaDePrecoRepository.GetTabelaDePrecoAtivaAsync()
        ?? throw new Exception(CodigoErrors.RegistroNotFound);
        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, clienteId);

        pedido.ProcessarItensPedido(pedidoCreateDto.PedidosPorPeso, pedidoCreateDto.PedidosPorTamanho, tabelaDePreco);

        await _pedidoRepository.AddAsync(pedido);

        await _processarPedidoService.ProcessarCreateAsync(pedido.Id);

        return new PedidoViewModel().ForModel(pedido);
    }

    public async Task<bool> DeletePedidoAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        return await _pedidoRepository.DeleteAsync(pedido);
    }

    public async Task<byte[]> DownloadPedidoPdfAsync(Guid pedidoId)
    {
        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var pdf = PedidoPdfService.GeneratePdfAsync(pedido);

        return pdf;
    }

    public async Task<PaginacaoViewModel<PedidoViewModel>> GetPaginacaoAsync(PaginacaoPedidoDto paginacaoPedidoDto)
    {
        var paginacao = await _pedidoRepository.GetPaginacaoPedidoAsync(paginacaoPedidoDto);

        return new PaginacaoViewModel<PedidoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new PedidoViewModel().ForModel(x)).ToList()
        };
    }

    public async Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido, Guid usuarioId)
    {
        var pedidos = await _pedidoRepository.GetPedidosByUsuarioIdAsync(usuarioId, statusPedido);
        return pedidos
            .Select(x => new PedidoViewModel().ForModel(x))
            .ToList();
    }

    public async Task ReenviarPedidoViaEmailAsync(Guid pedidoId)
    {
        await _processarPedidoService.ProcessarCreateAsync(pedidoId);
    }

    public async Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(updateStatusPedidoDto.PedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        pedido.UpdateStatus(updateStatusPedidoDto.StatusPedido);

        await _pedidoRepository.UpdateAsync(pedido);

        if (pedido.StatusPedido == StatusPedido.Entregue)
        {
            _processarPedidoService.ProcessarProdutosMaisVendidosAsync(pedido);
        }

        return new PedidoViewModel().ForModel(pedido);
    }
}
