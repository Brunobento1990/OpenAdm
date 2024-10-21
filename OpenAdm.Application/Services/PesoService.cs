using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Pesos;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;
using OpenAdm.Application.Dtos.Pesos;
using OpenAdm.Domain.Exceptions;

namespace OpenAdm.Application.Services;

public class PesoService : IPesoService
{
    private readonly IPesoRepository _pesoRepository;

    public PesoService(IPesoRepository pesoRepository)
    {
        _pesoRepository = pesoRepository;
    }

    public async Task<PesoViewModel> CreatePesoAsync(CreatePesoDto createPesoDto)
    {
        var peso = createPesoDto.ToEntity();
        await _pesoRepository.AddAsync(peso);
        return new PesoViewModel().ToModel(peso);
    }

    public async Task DeletePesoAsync(Guid id)
    {
        var peso = await _pesoRepository.GetPesoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o peso");

        await _pesoRepository.DeleteAsync(peso);
    }

    public async Task<PaginacaoViewModel<PesoViewModel>> GetPaginacaoAsync(PaginacaoPesoDto paginacaoPesoDto)
    {
        var paginacao = await _pesoRepository.GetPaginacaoPesoAsync(paginacaoPesoDto);

        return new PaginacaoViewModel<PesoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new PesoViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<IDictionary<Guid, PesoViewModel>> GetPesosByPesosIdsViewModelAsync(IList<Guid> pesosIds)
    {
        var pesos = await _pesoRepository.GetDictionaryPesosByIdsAsync(pesosIds);

        var retorno = new Dictionary<Guid, PesoViewModel>();

        foreach (var peso in pesos)
        {
            retorno.TryAdd(peso.Key, new PesoViewModel().ToModel(peso.Value));
        }

        return retorno;
    }

    public async Task<IList<PesoViewModel>> GetPesosViewModelAsync()
    {
        var pesos = await _pesoRepository.GetPesosAsync();

        return pesos.Select(x => new PesoViewModel().ToModel(x)).ToList();
    }

    public async Task<PesoViewModel> GetPesoViewModelAsync(Guid id)
    {
        var peso = await _pesoRepository.GetPesoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o peso");

        return new PesoViewModel().ToModel(peso);
    }

    public async Task<PesoViewModel> UpdatePesoAsync(UpdatePesoDto updatePesoDto)
    {
        var peso = await _pesoRepository.GetPesoByIdAsync(updatePesoDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar o peso");

        peso.Update(updatePesoDto.Descricao, updatePesoDto.PesoReal);

        await _pesoRepository.UpdateAsync(peso);

        return new PesoViewModel().ToModel(peso);
    }
}
