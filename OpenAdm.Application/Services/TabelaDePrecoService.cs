using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.TabelaDePrecos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class TabelaDePrecoService : ITabelaDePrecoService
{
    private readonly ITabelaDePrecoRepository _tabelaDePrecoRepository;

    public TabelaDePrecoService(ITabelaDePrecoRepository tabelaDePrecoRepository)
    {
        _tabelaDePrecoRepository = tabelaDePrecoRepository;
    }

    public async Task<PaginacaoViewModel<TabelaDePrecoViewModel>> GetPaginacaoTabelaViewModelAsync(
        PaginacaoTabelaDePrecoDto paginacaoTabelaDePrecoDto)
    {
        var paginacao = await _tabelaDePrecoRepository.GetPaginacaoAsync(paginacaoTabelaDePrecoDto);

        return new PaginacaoViewModel<TabelaDePrecoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new TabelaDePrecoViewModel().ToEntity(x)).ToList()
        };
    }
}
