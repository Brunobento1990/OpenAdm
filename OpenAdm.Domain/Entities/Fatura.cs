using OpenAdm.Domain.Entities.Bases;
using OpenAdm.Domain.Enuns;

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

    public decimal Total { get; private set; }

    public decimal ValorAPagarAReceber
    {
        get { return Parcelas.Sum(x => x.ValorAPagarAReceber); }
    }

    public decimal ValorPagoRecebido
    {
        get { return Parcelas.Sum(x => x.ValorPagoRecebido); }
    }

    public void Fechar()
    {
        DataDeFechamento = DateTime.Now;
        Status = StatusFaturaEnum.Paga;
        DataDeAtualizacao = DateTime.Now;
    }

    public void PagaParcialmente()
    {
        Status = StatusFaturaEnum.Paga_Parcialmente;
        DataDeAtualizacao = DateTime.Now;
    }
}
