using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Tamanhos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Tamanhos;
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
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        await _tamanhoRepository.DeleteAsync(tamanho);
    }

    public async Task<PaginacaoViewModel<TamanhoViewModel>> GetPaginacaoAsync(PaginacaoTamanhoDto paginacaoTamanhoDto)
    {
        var paginacao = await _tamanhoRepository.GetPaginacaoTamanhoAsync(paginacaoTamanhoDto);

        return new PaginacaoViewModel<TamanhoViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new TamanhoViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<TamanhoViewModel> GetTamanhoViewModelAsync(Guid id)
    {
        var tamanho = await _tamanhoRepository.GetTamanhoByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        return new TamanhoViewModel().ToModel(tamanho);
    }

    public async Task<TamanhoViewModel> UpdateTamanhoAsync(UpdateTamanhoDto updateTamanhoDto)
    {
        var tamanho = await _tamanhoRepository.GetTamanhoByIdAsync(updateTamanhoDto.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        tamanho.Update(updateTamanhoDto.Descricao);

        await _tamanhoRepository.UpdateAsync(tamanho);
        return new TamanhoViewModel().ToModel(tamanho);
    }
}
