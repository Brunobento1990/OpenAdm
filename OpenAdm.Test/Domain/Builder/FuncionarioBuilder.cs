using Domain.Pkg.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class FuncionarioBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private string _email;
    private string _senha;
    private string _nome;
    private string? _telefone;
    private byte[]? _avatar;

    public FuncionarioBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _email = faker.Person.Email;
        _senha = faker.Lorem.Text();
        _nome = faker.Person.FirstName;
        _telefone = faker.Person.Phone;
        _avatar = null;
    }

    public static FuncionarioBuilder Init() => new();

    public FuncionarioBuilder SemEmail(string email)
    {
        _email = email;
        return this;
    }

    public FuncionarioBuilder TelefoneInvalido()
    {
        _telefone = "5156165165165448484891516616516515616";
        return this;
    }

    public FuncionarioBuilder TelefoneInvalidoComLetras()
    {
        _telefone = "adfdfwrwg";
        return this;
    }

    public FuncionarioBuilder SemSenha(string senha)
    {
        _senha = senha;
        return this;
    }

    public FuncionarioBuilder SemNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public Funcionario Build()
    {
        return new(_id, _created, _update, _numero, _email, _senha, _nome, _telefone, _avatar);
    }
}
