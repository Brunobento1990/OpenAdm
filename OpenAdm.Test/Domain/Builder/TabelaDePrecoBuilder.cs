using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model.Pedidos;

namespace OpenAdm.Test.Domain.Builder;

public class TabelaDePrecoBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private readonly bool _ativaEcommerce;
    private string _descricao;

    public TabelaDePrecoBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _descricao = faker.Person.FirstName;
        _ativaEcommerce = true;
    }

    public static TabelaDePrecoBuilder Init() => new();

    public TabelaDePrecoBuilder SemDescricao(string descricao)
    {
        _descricao = descricao;
        return this;
    }

    public TabelaDePreco Build()
    {
        return new TabelaDePreco(_id, _created, _update, _numero, _descricao, _ativaEcommerce);
    }
}
