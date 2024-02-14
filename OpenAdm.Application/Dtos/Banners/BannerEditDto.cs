using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Banners;

public class BannerEditDto
{
    public Guid Id { get; set; }
    [Required(ErrorMessage = "Informe a imagem do banner!")]
    public string Foto { get; set; } = string.Empty;

    public bool? Ativo { get; set; }
}
