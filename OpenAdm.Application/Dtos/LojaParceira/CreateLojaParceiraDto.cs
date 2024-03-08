using Domain.Pkg.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.LojaParceira;

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
    public string? Foto { get; set; }

    public LojasParceiras ToEntity(string? nomeFoto)
    {
        var date = DateTime.Now;

        return new LojasParceiras(
            Guid.NewGuid(),
            date,
            date,
            0,
            Nome,
            nomeFoto,
            Foto,
            Instagram,
            Facebook,
            Endereco,
            Contato);
    }
}
