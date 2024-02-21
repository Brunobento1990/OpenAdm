using Domain.Pkg.Entities;
using Domain.Pkg.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenAdm.Application.Dtos.Categorias;

public class CategoriaCreateDto
{
    [Required]
    [MaxLength(255)]
    public string Descricao { get; set; } = string.Empty;

    public string? Foto { get; set; }

    public Categoria ToEntity()
    {
        var date = DateTime.Now;

        var byteCount = Encoding.UTF8.GetByteCount(Foto);
        byte[] foto = new byte[byteCount];

        if (string.IsNullOrWhiteSpace(Foto)
            || !Encoding.UTF8.TryGetBytes(Foto, foto, out byteCount))
            throw new ExceptionApi("A foto do banner é inválida!");

        return new Categoria(Guid.NewGuid(), date, date, 0, Descricao, foto);
    }
}
