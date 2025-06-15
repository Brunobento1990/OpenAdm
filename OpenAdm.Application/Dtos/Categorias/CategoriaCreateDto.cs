using System.ComponentModel.DataAnnotations;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.Categorias;

public class CategoriaCreateDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public string? NovaFoto { get; set; }

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(Descricao))
        {
            throw new ExceptionApi("Informe a descrição da categoria");
        }

        if (Descricao.Length > 255)
        {
            throw new ExceptionApi("A descrição da categoria deve ter no máximo 25 caracteres");
        }
    }
}
