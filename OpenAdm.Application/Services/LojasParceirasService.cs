using OpenAdm.Application.Dtos.LojasParceiras;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.LojasParceira;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class LojasParceirasService : ILojasParceirasService
{
    private readonly ILojasParceirasRepository _lojasParceirasRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public LojasParceirasService(ILojasParceirasRepository lojasParceirasRepository, IUploadImageBlobClient uploadImageBlobClient, IUsuarioAutenticado usuarioAutenticado)
    {
        _lojasParceirasRepository = lojasParceirasRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<LojasParceirasViewModel> CreateLojaParceiraAsync(CreateLojaParceiraDto createLojaParceiraDto)
    {
        var nomeFoto = string.Empty;
        if (!string.IsNullOrWhiteSpace(createLojaParceiraDto.NovaFoto))
        {
            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            createLojaParceiraDto.NovaFoto = await _uploadImageBlobClient.UploadImageAsync(createLojaParceiraDto.NovaFoto, nomeFoto);
        }

        var proximoNumero = await _lojasParceirasRepository.ProximoNumeroAsync(_usuarioAutenticado.ParceiroId);
        var lojaParceira = createLojaParceiraDto.ToEntity(nomeFoto, proximoNumero, _usuarioAutenticado.ParceiroId);

        await _lojasParceirasRepository.AddAsync(lojaParceira);
        await _lojasParceirasRepository.SaveChangesAsync();

        return new LojasParceirasViewModel().ToModel(lojaParceira);
    }

    public async Task DeleteLojaParceiraAsync(Guid id)
    {
        var lojaParceira = await GetLojaAsync(id);

        if (!string.IsNullOrWhiteSpace(lojaParceira.NomeFoto))
        {
            await _uploadImageBlobClient.DeleteImageAsync(lojaParceira.NomeFoto);
        }

        _lojasParceirasRepository.Delete(lojaParceira);
        await _lojasParceirasRepository.SaveChangesAsync();
    }

    public async Task<LojasParceirasViewModel> GetLojasParceirasViewModelAsync(Guid id)
    {
        var lojaParceira = await GetLojaAsync(id);

        return new LojasParceirasViewModel().ToModel(lojaParceira);
    }

    public async Task<PaginacaoViewModel<LojasParceirasViewModel>> GetPaginacaoAsync(FilterModel<LojaParceira> paginacaoLojasParceirasDto)
    {
        paginacaoLojasParceirasDto.ParceiroId = _usuarioAutenticado.ParceiroId;
        var paginacao = await _lojasParceirasRepository.PaginacaoAsync(paginacaoLojasParceirasDto);

        return new PaginacaoViewModel<LojasParceirasViewModel>()
        {
            TotalDeRegistros = paginacao.TotalDeRegistros,
            Values = paginacao
                .Values
                .Select(x =>
                    new LojasParceirasViewModel().ToModel(x))
                .ToList(),
            TotalPaginas = paginacao.TotalPaginas,
        };
    }

    public async Task<IList<string?>> ListLojasParceirasViewModelAsync()
    {
        return await _lojasParceirasRepository.GetFotosLojasParceirasAsync(_usuarioAutenticado.ParceiroId);
    }

    public async Task<IList<LojasParceirasViewModel>> TodasLojasAsync()
    {
        var lojasParceiras = await _lojasParceirasRepository.GetLojasParceirasAsync(_usuarioAutenticado.ParceiroId);

        return lojasParceiras.Select(x => new LojasParceirasViewModel().ToModel(x)).ToList();
    }

    public async Task<LojasParceirasViewModel> UpdateLojaParceiraAsync(UpdateLojaParceiraDto updateLojaParceiraDto)
    {
        var lojaParceira = await GetLojaAsync(updateLojaParceiraDto.Id);

        var foto = lojaParceira.Foto;
        var nomeFoto = lojaParceira.NomeFoto;

        if (!string.IsNullOrWhiteSpace(updateLojaParceiraDto.NovaFoto))
        {
            if (!string.IsNullOrWhiteSpace(nomeFoto))
            {
                var resultDeleteFoto = await _uploadImageBlobClient.DeleteImageAsync(nomeFoto);
                if (!resultDeleteFoto)
                    throw new ExceptionApi("Não foi possível excluir a foto da loja, tente novamente mais tarde, ou entre em contato com o suporte!");

                nomeFoto = $"{Guid.NewGuid()}.jpeg";

                foto = await _uploadImageBlobClient.UploadImageAsync(updateLojaParceiraDto.NovaFoto, nomeFoto);
            }
        }

        lojaParceira.Update(
            updateLojaParceiraDto.Nome,
            nomeFoto,
            foto,
            updateLojaParceiraDto.Instagram,
            updateLojaParceiraDto.Facebook,
            updateLojaParceiraDto.Endereco,
            updateLojaParceiraDto.Contato);

        _lojasParceirasRepository.Update(lojaParceira);
        await _lojasParceirasRepository.SaveChangesAsync();

        return new LojasParceirasViewModel().ToModel(lojaParceira);
    }

    private async Task<LojaParceira> GetLojaAsync(Guid id)
    {
        return await _lojasParceirasRepository.GetLojaParceiraByIdAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar a loja!");
    }
}
