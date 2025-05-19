using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Dtos.Banners;

public class BannerCreateDto
{
    public string NovaFoto { get; set; } = string.Empty;

    public void Validar()
    {
        if (string.IsNullOrWhiteSpace(NovaFoto))
        {
            throw new ExceptionApi("Informe a foto do banner");
        }
    }

    public Banner ToEntity(string nomeFoto, string foto)
    {
        if (string.IsNullOrWhiteSpace(foto))
        {
            throw new ExceptionApi("Informe a foto do banner");
        }

        NovaFoto = foto;
        var date = DateTime.Now;
        return new Banner(Guid.NewGuid(), date, date, 0, NovaFoto, true, nomeFoto);
    }
}
