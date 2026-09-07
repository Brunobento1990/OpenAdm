using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.LinksBio;

public sealed class LinkBioConfiguracaoDto
{
    [Required]
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? CorDeFundo { get; set; }
    public string? CorPrincipal { get; set; }
    public string? BackgroundImage { get; set; }
    public bool Ativo { get; set; }
}
