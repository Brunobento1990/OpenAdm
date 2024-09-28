using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.Banners;

public class BannerCreateDto
{
    public string Foto { get; set; } = string.Empty;

    public Banner ToEntity(string nomeFoto, string foto)
    {
        if (string.IsNullOrWhiteSpace(foto))
        {
            throw new ExceptionApi("Informe a foto do banner");
        }

        Foto = foto;
        var date = DateTime.Now;
        return new Banner(Guid.NewGuid(), date, date, 0, Foto, true, nomeFoto);
    }
}
