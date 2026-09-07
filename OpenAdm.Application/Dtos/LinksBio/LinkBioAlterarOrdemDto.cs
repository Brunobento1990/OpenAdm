using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.LinksBio;

public sealed class LinkBioAlterarOrdemDto
{
    [Required]
    public Guid Id { get; set; }
    public int Ordem { get; set; }
}
