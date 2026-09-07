using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.LinksBio;

public sealed class LinkBioConfiguracaoViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? CorDeFundo { get; set; }
    public string? CorPrincipal { get; set; }
    public string? BackgroundImage { get; set; }
    public bool Ativo { get; set; }
    public IList<LinkBioItemViewModel> Links { get; set; } = [];
}

public sealed class LinkBioItemViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icone { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; }
}

public sealed class LinkBioPaginaPublicaViewModel
{
    public string NomeFantasia { get; set; } = string.Empty;
    public string? Logo { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? CorDeFundo { get; set; }
    public string? CorPrincipal { get; set; }
    public string? BackgroundImage { get; set; }
    public IList<LinkBioItemPublicoViewModel> Links { get; set; } = [];
}

public sealed class LinkBioItemPublicoViewModel
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public string? Icone { get; set; }
    public int Ordem { get; set; }
}

public sealed class LinkBioIndicadoresViewModel
{
    public int Visualizacoes { get; set; }
    public int Cliques { get; set; }
    public IList<LinkBioEventoViewModel> Eventos { get; set; } = [];
    public IList<LinkBioMaisClicadoViewModel> LinksMaisClicados { get; set; } = [];
}

public sealed class LinkBioEventoViewModel
{
    public Guid Id { get; set; }
    public Guid? LinkId { get; set; }
    public TipoEventoLinkBioEnum Tipo { get; set; }
    public DateTime DataHora { get; set; }
}

public sealed class LinkBioMaisClicadoViewModel
{
    public Guid LinkId { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public int Quantidade { get; set; }
}
