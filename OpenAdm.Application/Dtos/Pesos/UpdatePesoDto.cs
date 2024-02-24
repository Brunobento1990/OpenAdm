using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Pesos;

public class UpdatePesoDto
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;
}
