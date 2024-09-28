using OpenAdm.Application.Models.MovimentacaoDeProdutos;

namespace OpenAdm.Application.Interfaces;

public interface IMovimentacaoDeProdutoRelatorioService
{
    byte[] ObterPdfAsync(
        IList<MovimentacaoDeProdutoRelatorio> movimentacaoDeProdutoRelatorios, 
        string nomeFantasia,
        DateTime dataInicial,
        DateTime dataFinal,
        string? logo);
}
