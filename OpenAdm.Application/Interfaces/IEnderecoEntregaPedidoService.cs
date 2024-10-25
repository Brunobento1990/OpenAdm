using OpenAdm.Application.Dtos.EnderecosDeEntregasPedidos;
using OpenAdm.Application.Models.EnderecosEntregasPedido;

namespace OpenAdm.Application.Interfaces;

public interface IEnderecoEntregaPedidoService
{
    Task<EnderecoEntregaPedidoViewModel> CreateAsync(EnderecoEntregaPedidoCreateDto enderecoEntregaPedidoCreateDto);
    Task<EnderecoEntregaPedidoViewModel?> GetByPedidoIdAsync(Guid pedidoId);
}
