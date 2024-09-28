using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ItensPedidoService : IItensPedidoService
{
    private readonly IItensPedidoRepository _itensPedidoRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly INotificarPedidoEditadoService _notificarPedidoEditadoService;

    public ItensPedidoService(
        IItensPedidoRepository itensPedidoRepository,
        IPedidoRepository pedidoRepository,
        INotificarPedidoEditadoService notificarPedidoEditadoService)
    {
        _itensPedidoRepository = itensPedidoRepository;
        _pedidoRepository = pedidoRepository;
        _notificarPedidoEditadoService = notificarPedidoEditadoService;
    }

    public async Task<bool> DeleteItemPedidoAsync(Guid id)
    {
        var item = await _itensPedidoRepository.GetItemPedidoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possivel localizar o item do pedido!");

        var pedido = await _pedidoRepository.GetPedidoByIdAsync(item.PedidoId)
            ?? throw new ExceptionApi("Não foi possivel localizar o pedido");

        if (pedido.ItensPedido.Count == 1)
        {
            throw new ExceptionApi("Não é possível excluir o último item do pedido!");
        }

        if (pedido.StatusPedido != StatusPedido.Aberto)
        {
            throw new ExceptionApi("Este pedido já está entregue!");
        }
        var result = await _itensPedidoRepository.DeleteAsync(item);
        if (result)
        {
            await _notificarPedidoEditadoService.NotificarAsync(pedido);
        }
        return result;
    }

    public async Task<ItensPedidoViewModel> EditarQuantidadeDoItemAsync(UpdateQuantidadeItemPedidoDto updateQuantidadeItemPedidoDto)
    {
        var item = await _itensPedidoRepository.GetItemPedidoByIdAsync(updateQuantidadeItemPedidoDto.Id)
            ?? throw new ExceptionApi("Não foi possivel localizar o item do pedido!");

        var pedido = await _pedidoRepository.GetPedidoByIdAsync(item.PedidoId)
            ?? throw new ExceptionApi("Não foi possivel localizar o pedido!");

        if (pedido.StatusPedido != StatusPedido.Aberto)
        {
            throw new ExceptionApi("Este pedido já está entregue!");
        }

        item.EditarQuantidade(updateQuantidadeItemPedidoDto.Quantidade);
        await _itensPedidoRepository.UpdateAsync(item);

        await _notificarPedidoEditadoService.NotificarAsync(pedido);

        return new ItensPedidoViewModel().ToModel(item);
    }

    public async Task<IList<ItensPedidoViewModel>> GetItensPedidoByPedidoIdAsync(Guid pedidoId)
    {
        var itens = await _itensPedidoRepository.GetItensPedidoByPedidoIdAsync(pedidoId);

        return itens.Select(x => new ItensPedidoViewModel().ToModel(x)).ToList();
    }
}
