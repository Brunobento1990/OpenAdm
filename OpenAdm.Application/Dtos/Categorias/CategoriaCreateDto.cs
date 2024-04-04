using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Categorias;

public class CategoriaCreateDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public string? Foto { get; set; }
}
