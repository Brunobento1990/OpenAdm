using Domain.Pkg.Exceptions;
using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Azure.Interfaces;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;
    public CategoriaService(ICategoriaRepository categoriaRepository, IUploadImageBlobClient uploadImageBlobClient)
    {
        _categoriaRepository = categoriaRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
    }

    public async Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto)
    {
        var nomeFoto = !string.IsNullOrWhiteSpace(categoriaCreateDto.Foto) ? $"{Guid.NewGuid()}.jpeg" : null;

        if (!string.IsNullOrWhiteSpace(nomeFoto) && categoriaCreateDto.Foto != null)
        {
            categoriaCreateDto.Foto = await _uploadImageBlobClient.UploadImageAsync(categoriaCreateDto.Foto, nomeFoto);
        }


        var categoria = categoriaCreateDto.ToEntity(nomeFoto);

        await _categoriaRepository.AddAsync(categoria);

        return new CategoriaViewModel().ToModel(categoria);
    }

    public async Task DeleteCategoriaAsync(Guid id)
    {
        var categoria = await _categoriaRepository.GetCategoriaAsync(id)
            ?? throw new ExceptionApi();

        if (!string.IsNullOrWhiteSpace(categoria.NomeFoto))
        {
            var resultDeleteBlob = await _uploadImageBlobClient.DeleteImageAsync(categoria.NomeFoto);

            if (!resultDeleteBlob)
                throw new ExceptionApi();
        }

        await _categoriaRepository.DeleteAsync(categoria);
    }

    public async Task<CategoriaViewModel> GetCategoriaAsync(Guid id)
    {
        var categoria = await _categoriaRepository.GetCategoriaAsync(id)
            ?? throw new ExceptionApi();

        return new CategoriaViewModel().ToModel(categoria);
    }

    public async Task<IList<CategoriaViewModel>> GetCategoriasAsync()
    {
        var categorias = await _categoriaRepository.GetCategoriasAsync();

        return categorias.Select(x => new CategoriaViewModel().ToModel(x)).ToList();
    }

    public async Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(PaginacaoCategoriaDto paginacaoCategoriaDto)
    {
        var paginacao = await _categoriaRepository.GetPaginacaoCategoriaAsync(paginacaoCategoriaDto);

        return new PaginacaoViewModel<CategoriaViewModel>
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new CategoriaViewModel().ToModel(x)).ToList()
        };
    }

    public async Task<CategoriaViewModel> UpdateCategoriaAsync(UpdateCategoriaDto updateCategoriaDto)
    {
        var categoria = await _categoriaRepository.GetCategoriaAsync(updateCategoriaDto.Id)
            ?? throw new ExceptionApi();

        var nomeFoto = categoria.NomeFoto;
        var foto = categoria.Foto;

        if(!string.IsNullOrWhiteSpace(updateCategoriaDto.Foto) && !updateCategoriaDto.Foto.StartsWith("https://"))
        {
            if (!string.IsNullOrWhiteSpace(categoria.NomeFoto))
            {
                var resultDeleteBlob = await _uploadImageBlobClient.DeleteImageAsync(categoria.NomeFoto);
                if (!resultDeleteBlob)
                    throw new ExceptionApi();
            }

            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            foto = await _uploadImageBlobClient.UploadImageAsync(updateCategoriaDto.Foto, nomeFoto);
        }

        categoria.Update(updateCategoriaDto.Descricao, foto, nomeFoto);

        await _categoriaRepository.UpdateAsync(categoria);

        return new CategoriaViewModel().ToModel(categoria);
    }
}
