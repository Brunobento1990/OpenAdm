using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class CategoriaBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private string _descricao;

    public CategoriaBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _descricao = faker.Name.JobDescriptor();
    }

    public static CategoriaBuilder Init() => new();

    public CategoriaBuilder SemDescricao(string descricao)
    {
        _descricao = descricao;
        return this;
    }

    public Categoria Build()
    {
        return new Categoria(_id, _created, _update, _numero, _descricao, null, null, false);
    }
}
