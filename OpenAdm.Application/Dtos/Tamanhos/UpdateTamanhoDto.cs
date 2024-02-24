using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Tamanhos;

public class UpdateTamanhoDto
{
    public Guid Id { get; set; }

    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;
}
