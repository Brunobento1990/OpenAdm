using OpenAdm.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Pesos;

public class CreatePesoDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;
    public decimal? PesoReal { get; set; }

    public Peso ToEntity()
    {
        var date = DateTime.Now;
        return new Peso(Guid.NewGuid(), date, date, 0, Descricao, PesoReal);
    }
}
