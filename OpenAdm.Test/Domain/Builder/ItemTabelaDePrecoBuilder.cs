using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class ItemTabelaDePrecoBuilder
{
    private Guid _produtoId = Guid.NewGuid();
    private Guid? _pesoId;
    private Guid? _tamanhoId;
    private Produto? _produto;

    public static ItemTabelaDePrecoBuilder Init() => new();

    public ItemTabelaDePrecoBuilder ComProduto(Produto produto)
    {
        _produto = produto;
        _produtoId = produto.Id;
        return this;
    }

    public ItemTabelaDePrecoBuilder ComProdutoId(Guid produtoId) { _produtoId = produtoId; return this; }
    public ItemTabelaDePrecoBuilder ComPesoId(Guid? pesoId) { _pesoId = pesoId; return this; }
    public ItemTabelaDePrecoBuilder ComTamanhoId(Guid? tamanhoId) { _tamanhoId = tamanhoId; return this; }

    public ItemTabelaDePreco Build()
    {
        var data = DateTime.UtcNow;
        var item = new ItemTabelaDePreco(
            Guid.NewGuid(), data, data, 0, _produtoId, 10, 10,
            Guid.NewGuid(), _tamanhoId, _pesoId);
        item.Produto = _produto!;
        return item;
    }
}
