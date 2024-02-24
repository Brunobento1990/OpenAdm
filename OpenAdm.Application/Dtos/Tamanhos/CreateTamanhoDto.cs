using Domain.Pkg.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Tamanhos;

public class CreateTamanhoDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public Tamanho ToEntity()
    {
        var date = DateTime.Now;
        return new Tamanho(Guid.NewGuid(), date, date, 0, Descricao);
    }
}
