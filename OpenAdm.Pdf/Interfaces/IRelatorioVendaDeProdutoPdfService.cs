using OpenAdm.Domain.Model;

namespace OpenAdm.Pdf.Interfaces;

public interface IRelatorioVendaDeProdutoPdfService
{
    byte[] Gerar(
        IEnumerable<RelatorioVendaDeProdutoModel> dados,
        string nomeFantasia,
        byte[]? logo,
        DateTime? dataInicial,
        DateTime? dataFinal,
        decimal quantidadeTotal,
        decimal valorTotal);
}
