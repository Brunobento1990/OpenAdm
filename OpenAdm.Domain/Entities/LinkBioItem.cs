using OpenAdm.Domain.Entities.Bases;

namespace OpenAdm.Domain.Entities;

public sealed class LinkBioItem : BaseEntity
{
    public LinkBioItem(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero,
        Guid linkBioConfiguracaoId, string titulo, string url, string? icone, int ordem, bool ativo)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        LinkBioConfiguracaoId = linkBioConfiguracaoId;
        Titulo = titulo;
        Url = url;
        Icone = icone;
        Ordem = ordem;
        Ativo = ativo;
    }

    public Guid LinkBioConfiguracaoId { get; private set; }
    public LinkBioConfiguracao LinkBioConfiguracao { get; set; } = null!;
    public string Titulo { get; private set; }
    public string Url { get; private set; }
    public string? Icone { get; private set; }
    public int Ordem { get; private set; }
    public bool Ativo { get; private set; }

    public void Atualizar(string titulo, string url, string? icone, int ordem, bool ativo)
    {
        Titulo = titulo;
        Url = url;
        Icone = icone;
        Ordem = ordem;
        Ativo = ativo;
        DataDeAtualizacao = DateTime.UtcNow;
    }

    public void AlterarStatus(bool ativo)
    {
        Ativo = ativo;
        DataDeAtualizacao = DateTime.UtcNow;
    }

    public void AlterarOrdem(int ordem)
    {
        Ordem = ordem;
        DataDeAtualizacao = DateTime.UtcNow;
    }
}
