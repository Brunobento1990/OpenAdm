using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.LinksBio;

public sealed class LinkBioItemCreateDto
{
    [Required]
    public string Titulo { get; set; } = string.Empty;
    [Required]
    public string Url { get; set; } = string.Empty;
    public string? Icone { get; set; }
    public int Ordem { get; set; }
    public bool Ativo { get; set; } = true;
}
