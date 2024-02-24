using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Application.Mensageria.Interfaces;
using Domain.Pkg.Entities;

namespace OpenAdm.Application.Services;

public class ProcessarPedidoService : IProcessarPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    private readonly IProducerGeneric<ProcessarPedidoModel> _producerGeneric;
    private readonly IProducerGeneric<IList<AddOrUpdateProdutosMaisVendidosDto>> _producerGenericProdutosMaisVendidos;

    public ProcessarPedidoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IProducerGeneric<ProcessarPedidoModel> producerGeneric,
        IPedidoRepository pedidoRepository,
        IProducerGeneric<IList<AddOrUpdateProdutosMaisVendidosDto>> producerGenericProdutosMaisVendidos)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _producerGeneric = producerGeneric;
        _pedidoRepository = pedidoRepository;
        _producerGenericProdutosMaisVendidos = producerGenericProdutosMaisVendidos;
    }

    public async Task ProcessarCreateAsync(Guid pedidoId)
    {
        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync()
            ?? throw new Exception("Configurações de pedido inválida!");

        var pedido = await _pedidoRepository.GetPedidoCompletoByIdAsync(pedidoId);

        if(pedido != null)
        {
            var processarPedidoModel = new ProcessarPedidoModel()
            {
                EmailEnvio = configuracoesDePedido.EmailDeEnvio,
                Pedido = pedido
            };

            foreach (var item in pedido.ItensPedido)
            {
                if (item.Pedido != null)
                    item.Pedido = null;

                if(item.Produto != null)
                {
                    item.Produto.Tamanhos = new();
                    item.Produto.Pesos = new();
                    item.Produto.ItensPedido = new();
                    item.Produto.ItensTabelaDePreco = new();
                }

                if(item.Tamanho != null)
                {
                    item.Tamanho.ItensPedido = new();
                }

                if (item.Peso != null)
                {
                    item.Peso.ItensPedido = new();
                }
            }

            _producerGeneric.Publish(processarPedidoModel, "pedido-create");
        }
    }

    public void ProcessarProdutosMaisVendidosAsync(Pedido pedido)
    {
        var addOrUpdateProdutosMaisVendidos = pedido.ItensPedido.Select(x => new AddOrUpdateProdutosMaisVendidosDto()
        {
            ProdutoId = x.ProdutoId,
            QuantidadeProdutos = x.Quantidade
        }).ToList();

        _producerGenericProdutosMaisVendidos.Publish(addOrUpdateProdutosMaisVendidos, "produtos-mais-vendidos");
    }
}
