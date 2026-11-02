namespace OpenAdm.Domain.Entities;

public sealed class PedidoHistorico
{
    public PedidoHistorico(Guid id, Guid pedidoId, DateTime dataDeCriacao, string descricao)
    {
        Id = id;
        PedidoId = pedidoId;
        DataDeCriacao = dataDeCriacao;
        Descricao = descricao;
    }

    public Guid Id { get; private set; }
    public Guid PedidoId { get; private set; }
    public DateTime DataDeCriacao { get; private set; }
    public string Descricao { get; private set; }
    public Pedido Pedido { get; set; } = null!;

    public static PedidoHistorico Novo(Guid pedidoId, string descricao)
        => new(Guid.NewGuid(), pedidoId, DateTime.UtcNow, descricao);
}
