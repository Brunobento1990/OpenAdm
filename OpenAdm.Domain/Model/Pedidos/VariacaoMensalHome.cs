namespace OpenAdm.Domain.Model.Pedidos;

public class VariacaoMensalHome
{
    public int Mes { get; set; }
    public decimal TotalAnoAtual { get; set; }
    public decimal TotalAnoAnterior { get; set; }
    public decimal Porcentagem { get; set; }
    public int AnoAtual { get; set; }
    public int AnoAnterior { get; set; }
}
