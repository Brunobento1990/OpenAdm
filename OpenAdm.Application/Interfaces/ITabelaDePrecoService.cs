using OpenAdm.Application.Models.TabelaDePrecos;

namespace OpenAdm.Application.Interfaces;

public interface ITabelaDePrecoService
{
    Task<TabelaDePrecoViewModel> GetPaginacaoTabelaViewModelAsync();
}
