using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Models.Estoques;

public class EstoqueViewModel : BaseModel
{
    public decimal Quantidade { get; set; }
    public decimal QuantidadeDisponivel { get; set; }
    public decimal QuantidadeReservada { get; set; }
    public Guid ProdutoId { get; set; }
    public Guid? PesoId { get; set; }
    public Guid? TamanhoId { get; set; }
    public string? Produto { get; set; }
    public string? Foto { get; set; }
    public string? Tamanho { get; set; }
    public string? Peso { get; set; }

    public static explicit operator EstoqueViewModel(Estoque estoque)
    {
        return new EstoqueViewModel
        {
            Id = estoque.Id,
            Tamanho = estoque.Tamanho?.Descricao,
            Peso = estoque.Peso?.Descricao,
            DataDeCriacao = estoque.DataDeCriacao,
            DataDeAtualizacao = estoque.DataDeAtualizacao,
            Produto = estoque.Produto?.Descricao,
            Quantidade = estoque.Quantidade,
            ProdutoId = estoque.ProdutoId,
            Numero = estoque.Numero,
            Foto = estoque.Produto?.UrlFoto,
            PesoId = estoque.PesoId,
            TamanhoId = estoque.TamanhoId,
            QuantidadeDisponivel = estoque.QuantidadeDisponivel,
            QuantidadeReservada = estoque.QuantidadeReservada
        };
    }

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
        QuantidadeDisponivel = estoque.QuantidadeDisponivel;
        QuantidadeReservada = estoque.QuantidadeReservada;
        return this;
    }
}