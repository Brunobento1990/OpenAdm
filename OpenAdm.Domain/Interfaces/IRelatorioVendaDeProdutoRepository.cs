using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IRelatorioVendaDeProdutoRepository
{
    Task<(ICollection<RelatorioVendaDeProdutoModel> Dados, int TotalPagina, decimal QuantidadeTotal,
        decimal ValorTotal)> ListarAsync(DateTime? dataInicial, DateTime? dataFinal, int skip, int? take, bool asc);
}
