using OpenAdm.Pdf.DTOs;

namespace OpenAdm.Pdf.Interfaces;

public interface IMovimentacaoDeProdutoRelatorioService
{
    byte[] ObterPdfAsync(
        IList<MovimentacaoDeProdutoRelatorioDTO> movimentacaoDeProdutoRelatorios,
        string nomeFantasia,
        DateTime dataInicial,
        DateTime dataFinal,
        string? logo,
        IList<RelatorioMovimentoDeProdutoTotalizacaoDTO> totalCategoria,
        IList<RelatorioMovimentoDeProdutoTotalizacaoDTO> totalPesos,
        IList<RelatorioMovimentoDeProdutoTotalizacaoDTO> totalTamanhos);
}
