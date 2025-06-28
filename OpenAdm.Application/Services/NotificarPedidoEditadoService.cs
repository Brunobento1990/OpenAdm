using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Services;

public sealed class NotificarPedidoEditadoService : INotificarPedidoEditadoService
{
    private readonly IConfiguracoesDePedidoService _configuracoesDePedidoService;
    //private readonly IProducerGeneric<ProcessarPedidoModel> _producerGeneric;

    public NotificarPedidoEditadoService(IConfiguracoesDePedidoService configuracoesDePedidoService)
    {
        _configuracoesDePedidoService = configuracoesDePedidoService;
    }

    public async Task NotificarAsync(Pedido pedido)
    {
        var configuracoesDePedido = await _configuracoesDePedidoService.GetConfiguracoesDePedidoAsync();
        pedido.ItensPedido = new List<ItemPedido>();
        var processarPedidoModel = new ProcessarPedidoModel()
        {
            EmailEnvio = configuracoesDePedido.EmailDeEnvio,
            Pedido = pedido
        };
        //_producerGeneric.Publish(processarPedidoModel, "pedido-editado");
    }
}
