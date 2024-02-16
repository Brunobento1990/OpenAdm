using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class ItensPedidoBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private Guid? _pesoId;
    private Guid? _tamanhoId;
    private Guid _produtoId;
    private Guid _pedidoId;
    private decimal _quantidade;
    private decimal _valorUnitario;

    public ItensPedidoBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();

        _pesoId = null;
        _tamanhoId = null;

        _produtoId = Guid.NewGuid();
        _pedidoId = Guid.NewGuid();
        _quantidade = faker.Random.Decimal(1, 100);
        _valorUnitario = faker.Random.Decimal(1, 100);
    }

    public static ItensPedidoBuilder Init() => new();

    public ItensPedidoBuilder SemQuantidade(int? quantidade)
    {
        _quantidade = quantidade == null ? 0 : quantidade.Value;
        return this;
    }

    public ItensPedidoBuilder SemValorUnitario(int? valorUnitario)
    {
        _valorUnitario = valorUnitario == null ? 0 : valorUnitario.Value;
        return this;
    }

    public ItensPedidoBuilder SemProdutoId(Guid? produtoId)
    {
        _produtoId = produtoId == null ? Guid.Empty : produtoId.Value;
        return this;
    }

    public ItensPedidoBuilder SemPedidoId(Guid? pedidoId)
    {
        _pedidoId = pedidoId == null ? Guid.Empty : pedidoId.Value;
        return this;
    }

    public ItensPedido Build()
    {
        return new ItensPedido(_id, _created, _update, _numero, _pesoId, _tamanhoId, _produtoId, _pedidoId, _valorUnitario, _quantidade);
    }

    public static IList<ItensPedido> BuildItens()
    {
        var item1 = Init().Build();
        var item2 = Init().Build();
        var item3 = Init().Build();

        return new List<ItensPedido>()
        {
            item1,
            item2,
            item3
        };
    }
}
