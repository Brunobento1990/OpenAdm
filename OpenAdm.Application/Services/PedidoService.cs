using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enums;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;
using System.Reactive.Linq;

namespace OpenAdm.Application.Services;

public class PedidoService(
    IPedidoRepository pedidoRepository, 
    ITokenService tokenService, 
    ITabelaDePrecoRepository tabelaDePrecoRepository)
    : IPedidoService
{
    private readonly ITabelaDePrecoRepository _tabelaDePrecoRepository = tabelaDePrecoRepository;
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;
    private readonly ITokenService _tokenService = tokenService;

    public async Task<PedidoViewModel> CreatePedidoAsync(PedidoCreateDto pedidoCreateDto)
    {
        var tabelaDePreco = await _tabelaDePrecoRepository.GetTabelaDePrecoAtivaAsync()
        ?? throw new Exception(CodigoErrors.RegistroNotFound);
        var clienteId = _tokenService.GetTokenUsuarioViewModel().Id;
        var date = DateTime.Now;
        var pedido = new Pedido(Guid.NewGuid(), date, date, 0, StatusPedido.Aberto, clienteId);

        pedido.ProcessarItensPedido(pedidoCreateDto.PedidosPorPeso, pedidoCreateDto.PedidosPorTamanho, tabelaDePreco);

        await _pedidoRepository.AddAsync(pedido);

        return new PedidoViewModel().ForModel(pedido);
    }

    public async Task<bool> DeletePedidoAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        return await _pedidoRepository.DeleteAsync(pedido);
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

    public async Task<List<PedidoViewModel>> GetPedidosUsuarioAsync(int statusPedido)
    {
        var usuarioId = _tokenService.GetTokenUsuarioViewModel().Id;
        var pedidos = await _pedidoRepository.GetPedidosByUsuarioIdAsync(usuarioId, statusPedido);
        return pedidos
            .Select(x => new PedidoViewModel().ForModel(x))
            .ToList();
    }

    public async Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(updateStatusPedidoDto.PedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        pedido.UpdateStatus(updateStatusPedidoDto.StatusPedido);

        await _pedidoRepository.UpdateAsync(pedido);

        return new PedidoViewModel().ForModel(pedido);
    }
}
