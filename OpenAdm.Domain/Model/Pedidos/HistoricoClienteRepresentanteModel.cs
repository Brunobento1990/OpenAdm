namespace OpenAdm.Domain.Model.Pedidos;

public sealed class HistoricoClienteRepresentanteModel
{
    public DateTime? DataUltimaCompra { get; set; }
    public decimal TicketMedio { get; set; }
    public string? ProdutoMaisComprado { get; set; }
    public decimal ValorUltimoPedido { get; set; }
}
