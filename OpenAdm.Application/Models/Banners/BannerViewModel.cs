using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Banners;

public class BannerViewModel : BaseModel
{
    public string Foto { get; set; } = string.Empty;
    public bool Ativo { get; set; }

    public BannerViewModel ToModel(Banner entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Foto = entity.Foto;
        Ativo = entity.Ativo;

        return this;
    }
}
