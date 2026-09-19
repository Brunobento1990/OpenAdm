using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Domain.Entities;

public sealed class Fatura : BaseEntity
{
    public Fatura(
        Guid id,
        DateTime dataDeCriacao,
        DateTime dataDeAtualizacao,
        long numero,
        StatusFaturaEnum status,
        Guid usuarioId,
        Guid? pedidoId,
        DateTime? dataDeFechamento,
        TipoFaturaEnum tipo,
        decimal total)
        : base(id, dataDeCriacao, dataDeAtualizacao, numero)
    {
        Status = status;
        UsuarioId = usuarioId;
        PedidoId = pedidoId;
        DataDeFechamento = dataDeFechamento;
        Tipo = tipo;
        Total = total;
    }

    public StatusFaturaEnum Status { get; private set; }
    public TipoFaturaEnum Tipo { get; private set; }
    public Guid UsuarioId { get; private set; }
    public Usuario Usuario { get; set; } = null!;
    public Guid? PedidoId { get; private set; }
    public bool Quitada => Tipo == TipoFaturaEnum.Bonificado || ValorPagoRecebido >= Total;
    public Pedido? Pedido { get; set; }
    public DateTime? DataDeFechamento { get; private set; }
    public IList<Parcela> Parcelas { get; set; } = [];
    public IList<FaturaHistorico> Historicos { get; set; } = [];

    public decimal Total { get; private set; }

    public decimal ValorAPagarAReceber
    {
        get { return Parcelas.Where(x => x.Ativo).Sum(x => x.ValorAPagarAReceber); }
    }

    public decimal ValorPagoRecebido
    {
        get { return Parcelas.Where(x => x.Ativo).Sum(x => x.ValorPagoRecebido); }
    }

    public void Fechar()
    {
        DataDeFechamento = DateTime.UtcNow;
        Status = StatusFaturaEnum.Paga;
        DataDeAtualizacao = DateTime.UtcNow;
    }

    public void PagaParcialmente()
    {
        Status = StatusFaturaEnum.Paga_Parcialmente;
        DataDeAtualizacao = DateTime.UtcNow;
    }

    public FaturaHistorico Cancelar()
    {
        if (ValorPagoRecebido > 0)
            throw new ExceptionApi(
                "Não é possível cancelar uma fatura com pagamento registrado!");

        if (Parcelas.Any(x => x.Ativo && !string.IsNullOrWhiteSpace(x.IdExterno)))
            throw new ExceptionApi(
                "Não é possível cancelar uma fatura com parcela integrada externamente!");

        foreach (var parcela in Parcelas.Where(x => x.Ativo))
            parcela.Inativar();

        Status = StatusFaturaEnum.Cancelada;
        DataDeAtualizacao = DateTime.UtcNow;
        return FaturaHistorico.Nova(Id, "Fatura cancelada pela exclusão do pedido.");
    }
}
