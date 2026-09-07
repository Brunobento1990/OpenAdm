using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Domain.Entities;

public sealed class LinkBioConfiguracao : BaseEntity
{
    public LinkBioConfiguracao(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero,
        Guid empresaId, string titulo, string? descricao, string? corDeFundo,
        string? corPrincipal, string? backgroundImage, string? nomeBackgroundImage, bool ativo)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        EmpresaId = empresaId;
        Titulo = titulo;
        Descricao = descricao;
        CorDeFundo = corDeFundo;
        CorPrincipal = corPrincipal;
        BackgroundImage = backgroundImage;
        NomeBackgroundImage = nomeBackgroundImage;
        Ativo = ativo;
    }

    public Guid EmpresaId { get; private set; }
    public EmpresaOpenAdm Empresa { get; set; } = null!;
    public string Titulo { get; private set; }
    public string? Descricao { get; private set; }
    public string? CorDeFundo { get; private set; }
    public string? CorPrincipal { get; private set; }
    public string? BackgroundImage { get; private set; }
    public string? NomeBackgroundImage { get; private set; }
    public bool Ativo { get; private set; }
    public IList<LinkBioItem> Links { get; set; } = [];

    public void Atualizar(string titulo, string? descricao, string? corDeFundo,
        string? corPrincipal, string? backgroundImage, string? nomeBackgroundImage, bool ativo)
    {
        Titulo = titulo;
        Descricao = descricao;
        CorDeFundo = corDeFundo;
        CorPrincipal = corPrincipal;
        BackgroundImage = backgroundImage;
        NomeBackgroundImage = nomeBackgroundImage;
        Ativo = ativo;
        DataDeAtualizacao = DateTime.UtcNow;
    }
}
