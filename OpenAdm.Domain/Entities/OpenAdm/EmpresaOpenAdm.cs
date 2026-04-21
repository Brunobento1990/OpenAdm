using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities.OpenAdm;

public sealed class EmpresaOpenAdm : BaseEntity
{
    public EmpresaOpenAdm(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        bool ativo,
        string urlEcommerce,
        string urlAdmin,
        string connectionString, TipoParcelaCobrancaEnum tipoParcelaCobranca)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Ativo = ativo;
        UrlEcommerce = urlEcommerce;
        UrlAdmin = urlAdmin;
        ConnectionString = connectionString;
        TipoParcelaCobranca = tipoParcelaCobranca;
    }

    public bool Ativo { get; private set; }
    public string UrlEcommerce { get; private set; }
    public string UrlAdmin { get; private set; }
    public string ConnectionString { get; private set; }
    public TipoParcelaCobrancaEnum TipoParcelaCobranca { get; private set; } = TipoParcelaCobrancaEnum.Gratis;
}
