using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities.OpenAdm;

public sealed class LinkEmpresa : BaseEntity
{
    public LinkEmpresa(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero,
        Guid empresaId, string url)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        EmpresaId = empresaId;
        Url = url;
    }

    public Guid EmpresaId { get; private set; }
    public EmpresaOpenAdm Empresa { get; set; } = null!;
    public string Url { get; private set; }
}
