using Domain.Pkg.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenAdm.Application.Dtos.Categorias;

public class CategoriaCreateDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public string? Foto { get; set; }

    public Categoria ToEntity(string? nomeFoto)
    {
        var date = DateTime.Now;

        return new Categoria(Guid.NewGuid(), date, date, 0, Descricao, Foto, nomeFoto);
    }
}
