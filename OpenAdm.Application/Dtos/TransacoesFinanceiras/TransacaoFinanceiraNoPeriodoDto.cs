namespace OpenAdm.Application.Dtos.TransacoesFinanceiras;

public class TransacaoFinanceiraNoPeriodoDto
{
    public DateTime DataInicial { get; set; }
    public DateTime DataFinal { get; set; }
    public Guid? PedidoId { get; set; }
    public Guid? ClienteId { get; set; }
}
