using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Produtos;

namespace OpenAdm.Application.Models.Pesos;

public class PesoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public decimal? PesoReal { get; set; }
    public PrecoProdutoViewModel? PrecoProdutoView { get; set; }
    public PesoViewModel ToModel(Peso entity)
    {
        Id = entity.Id;
        Descricao = entity.Descricao;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        PesoReal = entity.PesoReal;
        return this;
    }
}
