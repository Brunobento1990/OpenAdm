using OpenAdm.Application.Dtos.Pedidos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Enums;

namespace OpenAdm.Test.Domain.Builder;

public class PedidoBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private StatusPedido _statusPedido;
    private Guid _usuarioId;


    public PedidoBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _statusPedido = StatusPedido.Aberto;
        _usuarioId = Guid.NewGuid();
    }

    public static PedidoBuilder Init() => new();

    public PedidoBuilder ComStatusPedido(StatusPedido statusPedido)
    {
        _statusPedido = statusPedido;
        return this;
    }

    public Pedido Build()
    {
        var pedido = new Pedido(_id, _created, _update, _numero, _statusPedido, _usuarioId);
        pedido.Usuario = UsuarioBuilder.Init().Build();
        return pedido;
    }

    public UpdateStatusPedidoDto BuildStatusPedidoDto()
    {
        return new UpdateStatusPedidoDto()
        {
            PedidoId = _id,
            StatusPedido = _statusPedido
        };
    }
}
