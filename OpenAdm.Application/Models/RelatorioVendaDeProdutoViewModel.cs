using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Models;

public class RelatorioVendaDeProdutoViewModel
{
    public IEnumerable<RelatorioVendaDeProdutoModel> Dados { get; set; } = [];
    public int TotalPagina { get; set; }
    public RelatorioVendaDeProdutoTotaisViewModel Totais { get; set; } = new();
}

public class RelatorioVendaDeProdutoTotaisViewModel
{
    public decimal QuantidadeTotal { get; set; }
    public decimal ValorTotal { get; set; }
}
