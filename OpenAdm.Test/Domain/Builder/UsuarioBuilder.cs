using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class UsuarioBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private string _email;
    private string _senha;
    private string _nome;
    private string? _telefone;
    private string? _cnpj;

    public UsuarioBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _email = faker.Person.Email;
        _senha = "12345678";
        _nome = faker.Person.FirstName;
        _telefone = "12345678911";
        _cnpj = faker.Person.Cpf();
    }

    public static UsuarioBuilder Init() => new();

    public Usuario Build()
    {
        return new Usuario(_id, _created, _update, _numero, _email, _senha, _nome, _telefone, _cnpj);
    }
}
