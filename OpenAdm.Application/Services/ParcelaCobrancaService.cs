using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class ParcelaCobrancaService : IParcelaCobrancaService
{
    private readonly IParcelaCobrancaRepository _parcelaCobrancaRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public ParcelaCobrancaService(IParcelaCobrancaRepository parcelaCobrancaRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        _parcelaCobrancaRepository = parcelaCobrancaRepository;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<PaginacaoViewModel<ParcelaCobrancaViewModel>> PaginacaoAsync(FilterModel<ParcelaCobranca> filter)
    {
        filter.ParceiroId = _parceiroAutenticado.Id;

        var paginacao = await _parcelaCobrancaRepository.PaginacaoAsync(filter);

        return new PaginacaoViewModel<ParcelaCobrancaViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => (ParcelaCobrancaViewModel)x).ToList()
        };
    }
}