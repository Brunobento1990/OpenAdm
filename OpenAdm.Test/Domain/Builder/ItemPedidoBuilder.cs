using Domain.Pkg.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class ItemPedidoBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private readonly Guid _pesoId;
    private readonly Guid _tamanho;
    private Guid _produtoId;
    private readonly Guid _pedidoId;
    private decimal _quantidade;

    public ItemPedidoBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _pesoId = Guid.NewGuid();
        _tamanho = Guid.NewGuid();
        _produtoId = Guid.NewGuid();
        _pedidoId = Guid.NewGuid();
        _quantidade = 1;
    }

    public static ItemPedidoBuilder Init() => new();

    public ItemPedidoBuilder SemQuantidade(decimal quantidade)
    {
        _quantidade = quantidade;
        return this;
    }

    public ItensPedido Build()
    {
        return new ItensPedido(
            id: _id,
            dataDeCriacao: _created,
            dataDeAtualizacao: _update,
            numero: _numero,
            pesoId: _pesoId,
            tamanhoId: _tamanho,
            produtoId: _produtoId,
            pedidoId: _pedidoId,
            valorUnitario: 1,
            quantidade: _quantidade);
    }
}
