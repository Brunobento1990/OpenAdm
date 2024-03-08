using Domain.Pkg.Entities;
using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Banners;

public class BannerCreateDto
{
    [Required(ErrorMessage = "Informe a imagem do banner!")]
    public string Foto { get; set; } = string.Empty;

    public Banner ToEntity(string nomeFoto, string foto)
    {
        Foto = foto;
        var date = DateTime.Now;
        return new Banner(Guid.NewGuid(), date, date, 0, Foto, true, nomeFoto);
    }
}
