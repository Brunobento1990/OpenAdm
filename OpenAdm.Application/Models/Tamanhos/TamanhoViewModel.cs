using OpenAdm.Domain.Entities;
using OpenAdm.Application.Models.Produtos;

namespace OpenAdm.Application.Models.Tamanhos;

public class TamanhoViewModel : BaseModel
{
    public decimal? PesoReal { get; set; }
    public decimal? Quantidade { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public bool TemEstoqueDisponivel { get; set; } = true;
    public decimal? AlturaReal { get; set; }
    public decimal? LarguraReal { get; set; }
    public decimal? ComprimentoReal { get; set; }
    public PrecoProdutoViewModel? PrecoProduto { get; set; }
    public TamanhoViewModel ToModel(Tamanho entity)
    {
        Id = entity.Id;
        DataDeCriacao = entity.DataDeCriacao;
        DataDeAtualizacao = entity.DataDeAtualizacao;
        Numero = entity.Numero;
        Descricao = entity.Descricao;
        PesoReal = entity.PesoReal;
        AlturaReal = entity.AlturaReal;
        LarguraReal = entity.LarguraReal;
        ComprimentoReal = entity.ComprimentoReal;
        return this;
    }
}
