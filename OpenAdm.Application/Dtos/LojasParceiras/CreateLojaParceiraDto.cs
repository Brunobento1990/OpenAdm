using OpenAdm.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.LojasParceiras;

public class CreateLojaParceiraDto
{
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
    public string? NovaFoto { get; set; }

    public LojaParceira ToEntity(string? nomeFoto)
    {
        var date = DateTime.Now;

        return new LojaParceira(
            Guid.NewGuid(),
            date,
            date,
            0,
            Nome,
            nomeFoto,
            NovaFoto,
            Instagram,
            Facebook,
            Endereco,
            Contato);
    }
}
