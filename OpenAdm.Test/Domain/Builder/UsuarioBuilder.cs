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
    private readonly string? _cnpj;
    private readonly string? _cpf;

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
        _cnpj = "12345678910";
        _cpf = "12345678910";
    }

    public static UsuarioBuilder Init() => new();

    public UsuarioBuilder SemEmail(string email)
    {
        _email = email;
        return this;
    }

    public UsuarioBuilder SemTelefone(string? telefone)
    {
        _telefone = telefone;
        return this;
    }

    public UsuarioBuilder SemSenha(string senha)
    {
        _senha = senha;
        return this;
    }

    public UsuarioBuilder SemNome(string nome)
    {
        _nome = nome;
        return this;
    }

    public UsuarioBuilder TelefoneInvalido()
    {
        _telefone = "5156165165165448484891516616516515616";
        return this;
    }

    public Usuario Build()
    {
        return new Usuario(_id, _created, _update, _numero, _email, _senha, _nome, _telefone, _cnpj, _cpf, true);
    }
}
