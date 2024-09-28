using OpenAdm.Domain.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class ParceiroBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private string _razaoSocial;
    private string _nomeFantasia;
    private string _cnpj;

    public ParceiroBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _razaoSocial = faker.Company.CompanyName();
        _nomeFantasia = faker.Company.CompanyName();
        _cnpj = "123";
    }

    public static ParceiroBuilder Init() => new();

    public Parceiro DeveCriarParceiro(string razaoSocial, string nomeFantasia, string cnpj)
    {
        return new(_id, _created, _update, _numero, razaoSocial, nomeFantasia, cnpj);
    }

    public ParceiroBuilder NaoDeveCriarSemCnpj(string cnpj)
    {
        _cnpj = cnpj;
        return this;
    }

    public ParceiroBuilder NaoDeveCriarSemNomeFantasia(string nomeFantasia)
    {
        _nomeFantasia = nomeFantasia;
        return this;
    }

    public ParceiroBuilder NaoDeveCriarSemRazaoSocial(string razaoSocial)
    {
        _razaoSocial = razaoSocial;
        return this;
    }

    public Parceiro Build()
    {
        return new(_id, _created, _update, _numero, _razaoSocial, _nomeFantasia, _cnpj);
    }
}
