using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Estoques;

public class EstoqueViewModel : BaseModel
{
    public decimal Quantidade { get; set; }
    public Guid ProdutoId { get; set; }
    public string? Produto { get; set; }
    public string? Foto { get; set; }
    public string? Tamanho { get; set; }
    public string? Peso { get; set; }

    public EstoqueViewModel ToModel(Estoque estoque, string? produto, string? tamanho, string? peso, string? foto)
    {
        Id = estoque.Id;
        Tamanho = tamanho;
        Peso = peso;
        DataDeCriacao = estoque.DataDeCriacao;
        DataDeAtualizacao = estoque.DataDeAtualizacao;
        Produto = produto;
        Quantidade = estoque.Quantidade;
        ProdutoId = estoque.ProdutoId;
        Numero = estoque.Numero;
        Foto = foto;
        return this;
    }
}
