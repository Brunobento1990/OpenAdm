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

    public ProcessarPedidoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository,
        IProducerGeneric<ProcessarPedidoModel> producerGeneric,
        IPedidoRepository pedidoRepository)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
        _producerGeneric = producerGeneric;
        _pedidoRepository = pedidoRepository;
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

    public async Task ProcessarProdutosMaisVendidosAsync(Pedido pedido)
    {
        Console.WriteLine(pedido.Id.ToString());
    }
}
