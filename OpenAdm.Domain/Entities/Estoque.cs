using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class Estoque : BaseEntity
{
    public Estoque(Guid id, DateTime dataDeCriacao, DateTime dataDeAtualizacao, long numero, Guid produtoId,
        decimal quantidade, Guid? tamanhoId, Guid? pesoId)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        ProdutoId = produtoId;
        Quantidade = quantidade;
        TamanhoId = tamanhoId;
        PesoId = pesoId;
    }

    public void UpdateEstoqueAtual(decimal quantidade)
    {
        Quantidade = quantidade;
    }

    public void UpdateEstoque(decimal quantidade, TipoMovimentacaoDeProduto tipoMovimentacaoDeProduto)
    {
        DataDeAtualizacao = DateTime.Now;
        if (tipoMovimentacaoDeProduto == TipoMovimentacaoDeProduto.Entrada)
        {
            Quantidade += quantidade;
            return;
        }

        Quantidade -= quantidade;
    }

    public void PedidoEntregue(decimal quantidade)
    {
        QuantidadeReservada -= quantidade;
        Quantidade -= quantidade;
        DataDeAtualizacao = DateTime.Now;
    }
    
    public void PedidoCanceladoExcluido(decimal quantidade)
    {
        QuantidadeReservada -= quantidade;
        DataDeAtualizacao = DateTime.Now;
    }

    public void ReservaEstoque(decimal quantidade)
    {
        DataDeAtualizacao = DateTime.Now;
        QuantidadeReservada += quantidade;
    }

    public Guid ProdutoId { get; private set; }
    public Produto Produto { get; set; } = null!;
    public Guid? TamanhoId { get; private set; }
    public Tamanho? Tamanho { get; set; }
    public Guid? PesoId { get; private set; }
    public Peso? Peso { get; set; }
    public decimal Quantidade { get; private set; }
    public decimal QuantidadeReservada { get; private set; }
    public decimal QuantidadeDisponivel => Quantidade - QuantidadeReservada;

    public static Estoque NovoEstoque(
        decimal quantidade,
        Guid produtoId,
        Guid? tamanhoId,
        Guid? pesoId)
    {
        return new Estoque(id: Guid.NewGuid(), dataDeCriacao: DateTime.Now,
            dataDeAtualizacao: DateTime.Now, numero: 0, produtoId: produtoId, quantidade: quantidade,
            tamanhoId: tamanhoId, pesoId: pesoId);
    }
}