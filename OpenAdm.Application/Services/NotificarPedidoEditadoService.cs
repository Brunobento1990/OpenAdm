using Domain.Pkg.Entities;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class NotificarPedidoEditadoService : INotificarPedidoEditadoService
{
    private readonly IConfiguracoesDePedidoRepository _configuracoesDePedidoRepository;
    //private readonly IProducerGeneric<ProcessarPedidoModel> _producerGeneric;

    public NotificarPedidoEditadoService(
        IConfiguracoesDePedidoRepository configuracoesDePedidoRepository)
    {
        _configuracoesDePedidoRepository = configuracoesDePedidoRepository;
    }

    public async Task NotificarAsync(Pedido pedido)
    {
        var configuracoesDePedido = await _configuracoesDePedidoRepository.GetConfiguracoesDePedidoAsync()
            ?? throw new Exception("Configurações de pedido inválida!");
        pedido.ItensPedido = new List<ItensPedido>();
        var processarPedidoModel = new ProcessarPedidoModel()
        {
            EmailEnvio = configuracoesDePedido.EmailDeEnvio,
            Pedido = pedido
        };
        //_producerGeneric.Publish(processarPedidoModel, "pedido-editado");
    }
}
