using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos;

public class BannerCreateDto
{
    [Required(ErrorMessage = "Informe a imagem do banner!")]
    public string Foto { get; set; } = string.Empty;
}
