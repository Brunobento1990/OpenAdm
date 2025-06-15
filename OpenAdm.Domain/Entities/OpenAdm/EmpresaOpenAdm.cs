using OpenAdm.Domain.Entities.Bases;

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
        string connectionString)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Ativo = ativo;
        UrlEcommerce = urlEcommerce;
        UrlAdmin = urlAdmin;
        ConnectionString = connectionString;
    }

    public bool Ativo { get; private set; }
    public string UrlEcommerce { get; private set; }
    public string UrlAdmin { get; private set; }
    public string ConnectionString { get; private set; }
}
