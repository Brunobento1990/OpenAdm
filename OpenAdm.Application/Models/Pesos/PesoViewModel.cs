using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.Pesos;

public class PesoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;

    public PesoViewModel ToModel(Peso entity)
    {
        Id = entity.Id;
        Descricao = entity.Descricao;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        return this;
    }
}
