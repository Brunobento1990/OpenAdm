using Domain.Pkg.Entities;

namespace OpenAdm.Test.Domain.Builder;

public class ConfiguracaoParceiroBuilder
{
    private readonly Guid _id;
    private readonly DateTime _created;
    private readonly DateTime _update;
    private readonly long _numero;
    private string _conexaoDb;
    private string _dominioSiteEcommerce;
    private string _dominioSiteAdm;
    private readonly bool _ativo;

    public ConfiguracaoParceiroBuilder()
    {
        _id = Guid.NewGuid();
        _created = DateTime.Now;
        _update = DateTime.Now;
        var faker = new Faker();
        _numero = faker.Random.Long(1, 10000);
        _conexaoDb = faker.Lorem.Paragraph(min: 10);
        _dominioSiteAdm = faker.Internet.DomainName();
        _dominioSiteEcommerce = faker.Internet.DomainName();
        _ativo = true;
    }


    public static ConfiguracaoParceiroBuilder Init() => new();

    public ConfiguracaoParceiroBuilder SemDominioSiteAdm(string dominio)
    {
        _dominioSiteAdm = dominio;
        return this;
    }

    public ConfiguracaoParceiroBuilder SemDominioSiteEcommerce(string dominio)
    {
        _dominioSiteEcommerce = dominio;
        return this;
    }

    public ConfiguracaoParceiroBuilder SemStringDb(string db)
    {
        _conexaoDb = db;
        return this;
    }

    public ConfiguracaoParceiro Build()
    {
        return new(_id, _created, _update, _numero, _conexaoDb, _dominioSiteAdm, _dominioSiteEcommerce, _ativo, Guid.NewGuid(), null);
    }
}
