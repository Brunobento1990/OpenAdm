using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Domain.Entities;

public sealed class Pedido : BaseEntity
{
    public Pedido(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        StatusPedido statusPedido,
        Guid usuarioId)
            : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        StatusPedido = statusPedido;
        UsuarioId = usuarioId;
    }

    public StatusPedido StatusPedido { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
    public EnderecoEntregaPedido? EnderecoEntrega { get; set; }
    public decimal ValorTotal { get { return ItensPedido.Sum(x => x.ValorTotal); } }
    public IList<ItemPedido> ItensPedido { get; set; } = [];

    public void UpdateStatus(StatusPedido statusPedido)
    {
        if (StatusPedido == StatusPedido.Entregue)
            throw new ExceptionApi("Não é possível modificar o status de um pedido entregue!");

        StatusPedido = statusPedido;
    }

    public void ProcessarItensPedido(IList<ItemPedidoModel> itensPedidoModels)
    {
        if (itensPedidoModels.Count == 0)
            throw new ExceptionApi("Informe os itens do pedido");

        ItensPedido = itensPedidoModels
            .Select(x =>
                new ItemPedido(
                    Guid.NewGuid(),
                    DataDeCriacao,
                    DataDeAtualizacao,
                    0,
                    x.PesoId,
                    x.TamanhoId,
                    x.ProdutoId,
                    Id,
                    x.ValorUnitario,
                    x.Quantidade))
            .ToList();
    }
}
