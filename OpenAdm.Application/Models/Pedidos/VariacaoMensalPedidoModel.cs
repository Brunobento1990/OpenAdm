namespace OpenAdm.Application.Models.Pedidos;

public class VariacaoMensalPedidoModel
{
    public string Mes { get; set; } = string.Empty;
    public decimal TotalAnoAnterior { get; set; }
    public decimal TotalAnoAtual { get; set; }
    public decimal Porcentagem { get; set; }
    public int AnoAtual { get; set; }
    public int AnoAnterior { get; set; }
}
