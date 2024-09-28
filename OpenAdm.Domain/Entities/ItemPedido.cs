using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class ItemPedido : BaseItem
{
    public ItemPedido(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        Guid? pesoId,
        Guid? tamanhoId,
        Guid produtoId,
        Guid pedidoId,
        decimal valorUnitario,
        decimal quantidade)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero, produtoId)
    {
        PesoId = pesoId;
        TamanhoId = tamanhoId;
        PedidoId = pedidoId;
        ValorUnitario = valorUnitario;
        Quantidade = quantidade;
    }

    public void EditarQuantidade(decimal quantidade)
    {
        Quantidade = quantidade;
    }

    public Guid? PesoId { get; private set; }
    public Peso? Peso { get; set; }
    public Guid? TamanhoId { get; private set; }
    public Tamanho? Tamanho { get; set; }
    public Guid PedidoId { get; private set; }
    public Pedido Pedido { get; set; } = null!;
    public decimal ValorUnitario { get; private set; }
    public decimal Quantidade { get; private set; }
    public decimal ValorTotal
    {
        get
        {
            return ValorUnitario * Quantidade;
        }
    }
}
