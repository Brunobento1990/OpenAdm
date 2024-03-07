using Domain.Pkg.Entities;

namespace OpenAdm.Application.Models.Estoques;

public class EstoqueViewModel : BaseModel
{
    public decimal Quantidade { get; set; }
    public Guid ProdutoId { get; set; }
    public string? Produto { get; set; }

    public EstoqueViewModel ToModel(Estoque estoque, string? produto)
    {
        Id = estoque.Id;
        DataDeCriacao = estoque.DataDeCriacao;
        DataDeAtualizacao = estoque.DataDeAtualizacao;
        Produto = produto;
        Quantidade = estoque.Quantidade;

        return this;
    }
}
