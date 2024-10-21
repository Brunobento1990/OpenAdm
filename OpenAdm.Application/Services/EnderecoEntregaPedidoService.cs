using OpenAdm.Application.Dtos.EnderecosDeEntregasPedidos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.EnderecosEntregasPedido;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class EnderecoEntregaPedidoService : IEnderecoEntregaPedidoService
{
    private readonly IEnderecoEntregaPedidoRepository _enderecoEntregaPedidoRepository;
    private readonly IPedidoService _pedidoService;

    public EnderecoEntregaPedidoService(
        IEnderecoEntregaPedidoRepository enderecoEntregaPedidoRepository,
        IPedidoService pedidoService)
    {
        _enderecoEntregaPedidoRepository = enderecoEntregaPedidoRepository;
        _pedidoService = pedidoService;
    }

    public async Task<EnderecoEntregaPedidoViewModel> CreateAsync(EnderecoEntregaPedidoCreateDto enderecoEntregaPedidoCreateDto)
    {
        var pedido = await _pedidoService.GetAsync(pedidoId: enderecoEntregaPedidoCreateDto.PedidoId);

        var endereco = new EnderecoEntregaPedido(
            cep: enderecoEntregaPedidoCreateDto.Cep,
            logradouro: enderecoEntregaPedidoCreateDto.Logradouro,
            bairro: enderecoEntregaPedidoCreateDto.Bairro,
            localidade: enderecoEntregaPedidoCreateDto.Localidade,
            complemento: enderecoEntregaPedidoCreateDto.Complemento,
            numero: enderecoEntregaPedidoCreateDto.Numero,
            uf: enderecoEntregaPedidoCreateDto.Uf,
            pedidoId: pedido.Id,
            valorFrete: enderecoEntregaPedidoCreateDto.ValorFrete,
            tipoFrete: enderecoEntregaPedidoCreateDto.TipoFrete,
            id: Guid.NewGuid());

        await _enderecoEntregaPedidoRepository.AddAsync(endereco);

        return (EnderecoEntregaPedidoViewModel)endereco;
    }
}
