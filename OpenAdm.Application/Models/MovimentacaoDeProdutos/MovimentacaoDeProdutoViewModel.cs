using Domain.Pkg.Entities;
using Domain.Pkg.Enum;

namespace OpenAdm.Application.Models.MovimentacaoDeProdutos;

public class MovimentacaoDeProdutoViewModel : BaseModel
{
    public decimal QuantidadeMovimentada { get; set; }
    public TipoMovimentacaoDeProduto TipoMovimentacaoDeProduto { get; set; }
    public string? Produto { get; set; }
    public string? Tamanho { get; set; }
    public string? Peso { get; set; }

    public MovimentacaoDeProdutoViewModel ToModel(MovimentacaoDeProduto movimentacaoDeProduto, string? produto, string? peso, string? tamanho)
    {
        Tamanho = tamanho;
        Peso = peso;
        Id = movimentacaoDeProduto.Id;
        DataDeCriacao = movimentacaoDeProduto.DataDeCriacao;
        DataDeAtualizacao = movimentacaoDeProduto.DataDeAtualizacao;
        Numero = movimentacaoDeProduto.Numero;
        QuantidadeMovimentada = movimentacaoDeProduto.QuantidadeMovimentada;
        TipoMovimentacaoDeProduto = movimentacaoDeProduto.TipoMovimentacaoDeProduto;
        Produto = produto;

        return this;
    }
}
