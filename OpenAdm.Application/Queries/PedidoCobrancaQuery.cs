namespace OpenAdm.Application.Queries;

public class PedidoCobrancaQuery
{
    public Guid PedidoId { get; set; }
    public decimal Valor { get; set; }
    public decimal? ValorFrete { get; set; }
    public decimal ValorTotal => Valor + (ValorFrete ?? 0);
    public string Cliente { get; set; } = string.Empty;
    public long NumeroPedido { get; set; }
}