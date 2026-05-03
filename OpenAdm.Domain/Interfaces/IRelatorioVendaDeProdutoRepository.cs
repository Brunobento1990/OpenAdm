using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IRelatorioVendaDeProdutoRepository
{
    Task<(ICollection<RelatorioVendaDeProdutoModel>, int TotalPagina)> ListarAsync(DateTime? dataInicial,
        DateTime? dataFinal, int skip, int take, bool asc);
}