using Domain.Pkg.Entities;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.LojaParceira;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.LojasParceira;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Azure.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class LojasParceirasService : ILojasParceirasService
{
    private readonly ILojasParceirasRepository _lojasParceirasRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;

    public LojasParceirasService(ILojasParceirasRepository lojasParceirasRepository, IUploadImageBlobClient uploadImageBlobClient)
    {
        _lojasParceirasRepository = lojasParceirasRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
    }

    public async Task<LojasParceirasViewModel> CreateLojaParceiraAsync(CreateLojaParceiraDto createLojaParceiraDto)
    {
        var nomeFoto = string.Empty;
        if (!string.IsNullOrWhiteSpace(createLojaParceiraDto.Foto))
        {
            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            createLojaParceiraDto.Foto = await _uploadImageBlobClient.UploadImageAsync(createLojaParceiraDto.Foto, nomeFoto);
        }

        var lojaParceira = createLojaParceiraDto.ToEntity(nomeFoto);

        await _lojasParceirasRepository.AddAsync(lojaParceira);

        return new LojasParceirasViewModel().ToModel(lojaParceira);
    }

    public async Task DeleteLojaParceiraAsync(Guid id)
    {
        var lojaParceira = await GetLojaAsync(id);

        if (!string.IsNullOrWhiteSpace(lojaParceira.NomeFoto))
        {
            await _uploadImageBlobClient.DeleteImageAsync(lojaParceira.NomeFoto);
        }

        await _lojasParceirasRepository.DeleteAsync(lojaParceira);
    }

    public async Task<LojasParceirasViewModel> GetLojasParceirasViewModelAsync(Guid id)
    {
        var lojaParceira = await GetLojaAsync(id);

        return new LojasParceirasViewModel().ToModel(lojaParceira);
    }

    public async Task<PaginacaoViewModel<LojasParceirasViewModel>> GetPaginacaoAsync(PaginacaoLojasParceirasDto paginacaoLojasParceirasDto)
    {
        var paginacao = await _lojasParceirasRepository.GetPaginacaoLojasParceirasAsync(paginacaoLojasParceirasDto);

        return new PaginacaoViewModel<LojasParceirasViewModel>()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao
                .Values
                .Select(x =>
                    new LojasParceirasViewModel().ToModel(x))
                .ToList()
        };
    }

    public async Task<IList<string?>> ListLojasParceirasViewModelAsync()
    {
        return await _lojasParceirasRepository.GetFotosLojasParceirasAsync();
    }

    public async Task<LojasParceirasViewModel> UpdateLojaParceiraAsync(UpdateLojaParceiraDto updateLojaParceiraDto)
    {
        var lojaParceira = await GetLojaAsync(updateLojaParceiraDto.Id);

        var foto = lojaParceira.Foto;
        var nomeFoto = lojaParceira.NomeFoto;

        if(!string.IsNullOrWhiteSpace(updateLojaParceiraDto.Foto) && !updateLojaParceiraDto.Foto.StartsWith("https://"))
        {
            if (!string.IsNullOrWhiteSpace(nomeFoto))
            {
                var resultDeleteFoto = await _uploadImageBlobClient.DeleteImageAsync(nomeFoto);
                if (!resultDeleteFoto)
                    throw new ExceptionApi("Não foi possível excluir a foto da loja, tente novamente mais tarde, ou entre em contato com o suporte!");

                nomeFoto = $"{Guid.NewGuid()}.jpeg";

                foto = await _uploadImageBlobClient.UploadImageAsync(updateLojaParceiraDto.Foto, nomeFoto);
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

        await _lojasParceirasRepository.UpdateAsync(lojaParceira);

        return new LojasParceirasViewModel().ToModel(lojaParceira);
    }

    private async Task<LojasParceiras> GetLojaAsync(Guid id)
    {
        return await _lojasParceirasRepository.GetLojaParceiraByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);
    }
}
