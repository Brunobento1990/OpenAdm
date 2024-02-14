using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Services;

public class PedidoService(IPedidoRepository pedidoRepository) 
    : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository = pedidoRepository;

    public async Task<bool> DeletePedidoAsync(Guid id)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(id)
            ?? throw new ExceptionApi(GenericError.RegistroNotFound);
    
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

    public async Task<PedidoViewModel> UpdateStatusPedidoAsync(UpdateStatusPedidoDto updateStatusPedidoDto)
    {
        var pedido = await _pedidoRepository.GetPedidoByIdAsync(updateStatusPedidoDto.PedidoId)
            ?? throw new ExceptionApi(GenericError.RegistroNotFound);

        pedido.UpdateStatus(updateStatusPedidoDto.StatusPedido);

        await _pedidoRepository.UpdateAsync(pedido);

        return new PedidoViewModel().ForModel(pedido);
    }
}
