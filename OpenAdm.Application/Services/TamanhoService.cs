using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tamanhos;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class TamanhoService : ITamanhoService
{
    private readonly ITamanhoRepository _tamanhoRepository;

    public TamanhoService(ITamanhoRepository tamanhoRepository)
    {
        _tamanhoRepository = tamanhoRepository;
    }

    public async Task<TamanhoViewModel> CreateTamanhoAsync(CreateTamanhoDto createTamanhoDto)
    {
        var tamanho = createTamanhoDto.ToEntity();
        await _tamanhoRepository.AddAsync(tamanho);
        return new TamanhoViewModel().ToModel(tamanho);
    }

    public async Task DeleteTamanhoAsync(Guid id)
    {
        var tamanho = await _tamanhoRepository.GetTamanhoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o tamanho");

        await _tamanhoRepository.DeleteAsync(tamanho);
    }

    public async Task<PaginacaoViewModel<TamanhoViewModel>> GetPaginacaoAsync(PaginacaoTamanhoDto paginacaoTamanhoDto)
    {
        var paginacao = await _tamanhoRepository.PaginacaoAsync(paginacaoTamanhoDto);

        return new PaginacaoViewModel<TamanhoViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => new TamanhoViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<IDictionary<Guid, TamanhoViewModel>> GetTamanhoPorIdsViewModelsAsync(IList<Guid> ids)
    {
        var tamanhos = await _tamanhoRepository.GetDictionaryTamanhosAsync(ids);
        var retorno = new Dictionary<Guid, TamanhoViewModel>();

        foreach (var tamanho in tamanhos)
        {
            retorno.TryAdd(tamanho.Key, new TamanhoViewModel().ToModel(tamanho.Value));
        }

        return retorno;
    }

    public async Task<TamanhoViewModel> GetTamanhoViewModelAsync(Guid id)
    {
        var tamanho = await _tamanhoRepository.GetTamanhoByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o tamanho");

        return new TamanhoViewModel().ToModel(tamanho);
    }

    public async Task<IList<TamanhoViewModel>> GetTamanhoViewModelsAsync()
    {
        var tamanhos = await _tamanhoRepository.GetTamanhosAsync();
        return tamanhos.Select(x => new TamanhoViewModel().ToModel(x)).ToList();
    }

    public async Task<TamanhoViewModel> UpdateTamanhoAsync(UpdateTamanhoDto updateTamanhoDto)
    {
        var tamanho = await _tamanhoRepository.GetTamanhoByIdAsync(updateTamanhoDto.Id)
            ?? throw new ExceptionApi("Não foi possível localizar o tamanho");

        tamanho.Update(updateTamanhoDto.Descricao, updateTamanhoDto.PesoReal);

        await _tamanhoRepository.UpdateAsync(tamanho);
        return new TamanhoViewModel().ToModel(tamanho);
    }
}
