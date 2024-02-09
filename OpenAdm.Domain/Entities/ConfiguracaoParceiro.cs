
using OpenAdm.Domain.Validations;

namespace OpenAdm.Domain.Entities;

public sealed class ConfiguracaoParceiro : BaseEntity
{
    public ConfiguracaoParceiro(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        string conexaoDb,
        string dominioSiteAdm,
        string dominioSiteEcommerce,
        bool ativo,
        Guid parceiroId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ValidationString.Validate(conexaoDb);
        ValidationString.Validate(dominioSiteAdm);
        ValidationString.Validate(dominioSiteEcommerce);

        DominioSiteEcommerce = dominioSiteEcommerce;
        DominioSiteAdm = dominioSiteAdm;
        ConexaoDb = conexaoDb;
        Ativo = ativo;
        ParceiroId = parceiroId;
    }

    public string ConexaoDb { get; private set; }
    public string DominioSiteAdm { get; private set; }
    public string DominioSiteEcommerce { get; private set; }
    public bool Ativo { get; private set; }
    public Guid ParceiroId { get; private set; }
    public Parceiro Parceiro { get; set; } = null!;
}
