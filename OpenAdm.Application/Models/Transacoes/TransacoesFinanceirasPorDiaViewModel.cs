namespace OpenAdm.Application.Models.Transacoes;

public class TransacoesFinanceirasPorDiaViewModel
{
    public IList<TransacaoFinanceiraViewModel> Transacoes { get; set; } = [];
    public decimal Total { get; set; }
}
