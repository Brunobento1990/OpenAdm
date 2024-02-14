
using OpenAdm.Domain.Enums;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Entities;

public sealed class Pedido : BaseEntity
{
    public Pedido(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, StatusPedido statusPedido, Guid usuarioId) : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        StatusPedido = statusPedido;
        UsuarioId = usuarioId;
    }

    public StatusPedido StatusPedido { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
    public decimal ValorTotal { get { return ItensPedido.Sum(x => x.ValorTotal); } }
    public List<ItensPedido> ItensPedido { get; set; } = new();

    public void UpdateStatus(StatusPedido statusPedido)
    {
        if (StatusPedido == StatusPedido.Entregue)
            throw new ExceptionApi(DomainErrorMessage.StatusDoPedidoEntregue);

        StatusPedido = statusPedido;
    }
}