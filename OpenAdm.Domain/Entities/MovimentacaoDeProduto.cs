using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Entities;

public sealed class MovimentacaoDeProduto : BaseEntity
{
    public MovimentacaoDeProduto(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        decimal quantidadeMovimentada,
        TipoMovimentacaoDeProduto tipoMovimentacaoDeProduto,
        Guid produtoId,
        Guid? tamanhoId,
        Guid? pesoId,
        string? observacao)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        QuantidadeMovimentada = quantidadeMovimentada;
        TipoMovimentacaoDeProduto = tipoMovimentacaoDeProduto;
        ProdutoId = produtoId;
        TamanhoId = tamanhoId;
        PesoId = pesoId;
        Observacao = observacao;
    }

    public decimal QuantidadeMovimentada { get; private set; }
    public TipoMovimentacaoDeProduto TipoMovimentacaoDeProduto { get; private set; }
    public Guid ProdutoId { get; private set; }
    public Guid? TamanhoId { get; private set; }
    public Guid? PesoId { get; private set; }
    public string? Observacao { get; private set; }
}
