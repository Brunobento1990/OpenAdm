using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.LojasParceiras;

public class UpdateLojaParceiraDto
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Instagram { get; set; }
    [MaxLength(500)]
    public string? Facebook { get; set; }
    [MaxLength(500)]
    public string? Endereco { get; set; }
    [MaxLength(20)]
    public string? Contato { get; set; }
    public string? Foto { get; set; }
}
