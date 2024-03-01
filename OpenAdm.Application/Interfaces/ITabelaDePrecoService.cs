using OpenAdm.Application.Models.TabelaDePrecos;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface ITabelaDePrecoService
{
    Task<PaginacaoViewModel<TabelaDePrecoViewModel>> GetPaginacaoTabelaViewModelAsync(PaginacaoTabelaDePrecoDto paginacaoTabelaDePrecoDto);
}
