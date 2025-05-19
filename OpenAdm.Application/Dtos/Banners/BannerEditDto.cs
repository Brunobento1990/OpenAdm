using System.ComponentModel.DataAnnotations;

namespace OpenAdm.Application.Dtos.Banners;

public class BannerEditDto
{
    public Guid Id { get; set; }
    public string? NovaFoto { get; set; }

    public bool? Ativo { get; set; }
}
