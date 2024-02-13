namespace OpenAdm.Domain.Entities;

public sealed class PesosProdutos
{
    public PesosProdutos(Guid id, Guid produtoId, Guid pesoId)
    {
        Id = id;
        ProdutoId = produtoId;
        PesoId = pesoId;
    }

    public Guid Id { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; set; } = null!;
    public Guid PesoId { get; private set; }
    public Peso Peso { get; set; } = null!;
}
