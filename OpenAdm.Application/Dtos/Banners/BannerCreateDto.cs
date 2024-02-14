using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace OpenAdm.Application.Dtos.Banners;

public class BannerCreateDto
{
    [Required(ErrorMessage = "Informe a imagem do banner!")]
    public string Foto { get; set; } = string.Empty;

    public Banner ToEntity()
    {
        var byteCount = Encoding.UTF8.GetByteCount(Foto);
        byte[] foto = new byte[byteCount];

        if (string.IsNullOrWhiteSpace(Foto)
            || !Encoding.UTF8.TryGetBytes(Foto, foto, out byteCount))
            throw new ExceptionApi("A foto do banner é inválida!");

        var date = DateTime.Now;
        return new Banner(Guid.NewGuid(), date, date, 0, foto, true);
    }
}
