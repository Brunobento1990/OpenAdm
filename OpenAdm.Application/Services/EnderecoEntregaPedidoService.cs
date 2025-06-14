using OpenAdm.Application.Dtos.EnderecosDeEntregasPedidos;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.EnderecosEntregasPedido;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class EnderecoEntregaPedidoService : IEnderecoEntregaPedidoService
{
    private readonly IEnderecoEntregaPedidoRepository _enderecoEntregaPedidoRepository;
    private readonly IPedidoService _pedidoService;
    private readonly IHttpClientCep _httpService;

    public EnderecoEntregaPedidoService(
        IEnderecoEntregaPedidoRepository enderecoEntregaPedidoRepository,
        IPedidoService pedidoService,
        IHttpClientCep httpService)
    {
        _enderecoEntregaPedidoRepository = enderecoEntregaPedidoRepository;
        _pedidoService = pedidoService;
        _httpService = httpService;
    }

    public async Task<EnderecoEntregaPedidoViewModel> CreateAsync(EnderecoEntregaPedidoCreateDto enderecoEntregaPedidoCreateDto)
    {
        _ = await _httpService.ConsultaCepAsync(enderecoEntregaPedidoCreateDto.Cep);
        var pedido = await _pedidoService.GetAsync(pedidoId: enderecoEntregaPedidoCreateDto.PedidoId);

        var endereco = new EnderecoEntregaPedido(
            cep: enderecoEntregaPedidoCreateDto.Cep,
            logradouro: enderecoEntregaPedidoCreateDto.Logradouro,
            bairro: enderecoEntregaPedidoCreateDto.Bairro,
            localidade: enderecoEntregaPedidoCreateDto.Localidade,
            complemento: enderecoEntregaPedidoCreateDto.Complemento ?? "",
            numero: enderecoEntregaPedidoCreateDto.Numero,
            uf: enderecoEntregaPedidoCreateDto.Uf,
            pedidoId: pedido.Id,
            valorFrete: enderecoEntregaPedidoCreateDto.ValorFrete ?? 0,
            tipoFrete: enderecoEntregaPedidoCreateDto.TipoFrete ?? "",
            id: Guid.NewGuid());

        await _enderecoEntregaPedidoRepository.AddAsync(endereco);

        return (EnderecoEntregaPedidoViewModel)endereco;
    }

    public async Task<EnderecoEntregaPedidoViewModel?> GetByPedidoIdAsync(Guid pedidoId)
    {
        var enderecoEntrega = await _enderecoEntregaPedidoRepository.GetByPedidoIdAsync(pedidoId);
        if (enderecoEntrega == null)
        {
            return null;
        }

        return (EnderecoEntregaPedidoViewModel)enderecoEntrega;
    }
}
