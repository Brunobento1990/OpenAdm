using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class LinkBioEvento : BaseEntity
{
    public LinkBioEvento(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero,
        Guid empresaId, Guid linkBioConfiguracaoId, Guid? linkBioItemId, TipoEventoLinkBioEnum tipo)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        EmpresaId = empresaId;
        LinkBioConfiguracaoId = linkBioConfiguracaoId;
        LinkBioItemId = linkBioItemId;
        Tipo = tipo;
    }

    public Guid EmpresaId { get; private set; }
    public EmpresaOpenAdm Empresa { get; set; } = null!;
    public Guid LinkBioConfiguracaoId { get; private set; }
    public LinkBioConfiguracao LinkBioConfiguracao { get; set; } = null!;
    public Guid? LinkBioItemId { get; private set; }
    public LinkBioItem? LinkBioItem { get; set; }
    public TipoEventoLinkBioEnum Tipo { get; private set; }
}
