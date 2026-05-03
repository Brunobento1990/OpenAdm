using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Models;

public class RelatorioVendaDeProdutoViewModel
{
    public IEnumerable<RelatorioVendaDeProdutoModel> Dados { get; set; } = [];
    public int TotalPagina { get; set; }
}