using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Categorias;

public class UpdateCategoriaDto
{
    public Guid Id { get; set; }
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public string? Foto { get; set; }
}
