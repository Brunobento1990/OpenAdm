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
                Logo = configuracoesDePedido.Logo,
                Pedido = pedido
            };
            _producerGeneric.Publish(processarPedidoModel, "pedido-create");
        }
    }

    public void ProcessarProdutosMaisVendidosAsync(Pedido pedido)
    {
        var processarPedidoModel = new ProcessarPedidoModel()
        {
            EmailEnvio = "",
            Pedido = pedido
        };

        foreach(var item in pedido.ItensPedido)
        {
            item.Pedido = null!;
            item.Produto = null!;
            item.Peso = null;
            item.Tamanho = null;
        }

        _producerGeneric.Publish(processarPedidoModel, "pedido-entregue");
    }
}
