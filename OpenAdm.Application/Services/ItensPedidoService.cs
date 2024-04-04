using Domain.Pkg.Enum;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.ItensPedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ItensPedidoService : IItensPedidoService
{
    private readonly IItensPedidoRepository _itensPedidoRepository;
    private readonly IPedidoRepository _pedidoRepository;

    public ItensPedidoService(
        IItensPedidoRepository itensPedidoRepository,
        IPedidoRepository pedidoRepository)
    {
        _itensPedidoRepository = itensPedidoRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task EditarQuantidadeDoItemAsync(UpdateQuantidadeItemPedidoDto updateQuantidadeItemPedidoDto)
    {
        var item = await _itensPedidoRepository.GetItemPedidoByIdAsync(updateQuantidadeItemPedidoDto.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var pedido = await _pedidoRepository.GetPedidoByIdAsync(item.PedidoId)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        if(pedido.StatusPedido != StatusPedido.Aberto)
        {
            throw new ExceptionApi("Este pedido já está entregue!");
        }

        item.EditarQuantidade(updateQuantidadeItemPedidoDto.Quantidade);
        await _itensPedidoRepository.UpdateAsync(item);
    }

    public async Task<IList<ItensPedidoViewModel>> GetItensPedidoByPedidoIdAsync(Guid pedidoId)
    {
        var itens = await _itensPedidoRepository.GetItensPedidoByPedidoIdAsync(pedidoId);

        return itens.Select(x => new ItensPedidoViewModel().ToModel(x)).ToList();
    }
}
