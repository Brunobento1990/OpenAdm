using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Produtos;

namespace OpenAdm.Application.Models.Pesos;

public class PesoViewModel : BaseModel
{
    public string Descricao { get; set; } = string.Empty;
    public decimal? PesoReal { get; set; }
    public decimal? Quantidade { get; set; }
    public decimal? AlturaReal { get; set; }
    public decimal? LarguraReal { get; set; }
    public decimal? ComprimentoReal { get; set; }
    public PrecoProdutoViewModel? PrecoProduto { get; set; }
    public bool TemEstoqueDisponivel { get; set; } = true;

    public PesoViewModel ToModel(Peso entity)
    {
        Id = entity.Id;
        Descricao = entity.Descricao;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        PesoReal = entity.PesoReal;
        AlturaReal = entity.AlturaReal;
        LarguraReal = entity.LarguraReal;
        ComprimentoReal = entity.ComprimentoReal;
        return this;
    }
}
