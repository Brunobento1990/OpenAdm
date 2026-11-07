using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pedidos;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Services;

public sealed class NotificarPedidoEditadoService : INotificarPedidoEditadoService
{
    private readonly IConfiguracaoDoSistemaService _configuracaoDoSistemaService;
    //private readonly IProducerGeneric<ProcessarPedidoModel> _producerGeneric;

    public NotificarPedidoEditadoService(IConfiguracaoDoSistemaService configuracaoDoSistemaService)
    {
        _configuracaoDoSistemaService = configuracaoDoSistemaService;
    }

    public async Task NotificarAsync(Pedido pedido)
    {
        var configuracoesDePedido = await _configuracaoDoSistemaService.GetConfiguracaoDoSistemaAsync();
        pedido.ItensPedido = new List<ItemPedido>();
        var processarPedidoModel = new ProcessarPedidoModel()
        {
            EmailEnvio = configuracoesDePedido.EmailParaEnvioNotificacao,
            Pedido = pedido
        };
        //_producerGeneric.Publish(processarPedidoModel, "pedido-editado");
    }
}
