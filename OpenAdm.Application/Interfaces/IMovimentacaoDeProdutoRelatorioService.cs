using OpenAdm.Application.Dtos.MovimentosDeProdutos;
using OpenAdm.Application.Models.MovimentacaoDeProdutos;

namespace OpenAdm.Application.Interfaces;

public interface IMovimentacaoDeProdutoRelatorioService
{
    byte[] ObterPdfAsync(
        IList<MovimentacaoDeProdutoRelatorio> movimentacaoDeProdutoRelatorios, 
        string nomeFantasia,
        DateTime dataInicial,
        DateTime dataFinal,
        string? logo,
        IList<RelatorioMovimentoDeProdutoTotalizacaoDto> totalCategoria,
        IList<RelatorioMovimentoDeProdutoTotalizacaoDto> totalPesos,
        IList<RelatorioMovimentoDeProdutoTotalizacaoDto> totalTamanhos);
}
