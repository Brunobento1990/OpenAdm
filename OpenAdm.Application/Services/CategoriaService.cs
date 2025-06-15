using OpenAdm.Application.Dtos.Categorias;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Mappers;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class CategoriaService : ICategoriaService
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;
    private const string ERRO_NOT_FOUND = "Não foi possível localizar a categoria!";
    public CategoriaService(ICategoriaRepository categoriaRepository, IUploadImageBlobClient uploadImageBlobClient)
    {
        _categoriaRepository = categoriaRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
    }

    public async Task<CategoriaViewModel> CreateCategoriaAsync(CategoriaCreateDto categoriaCreateDto)
    {
        categoriaCreateDto.Validar();
        var nomeFoto = !string.IsNullOrWhiteSpace(categoriaCreateDto.NovaFoto) ? $"{Guid.NewGuid()}.jpeg" : null;

        if (!string.IsNullOrWhiteSpace(categoriaCreateDto.NovaFoto) && !string.IsNullOrWhiteSpace(nomeFoto))
        {
            categoriaCreateDto.NovaFoto = await _uploadImageBlobClient.UploadImageAsync(categoriaCreateDto.NovaFoto, nomeFoto);
        }

        var categoria = EntityMapper.ToCategoriaCreate(categoriaCreateDto, nomeFoto);

        await _categoriaRepository.AddAsync(categoria);

        return new CategoriaViewModel().ToModel(categoria);
    }

    public async Task DeleteCategoriaAsync(Guid id)
    {
        var categoria = await _categoriaRepository.GetCategoriaAsync(id)
            ?? throw new ExceptionApi(ERRO_NOT_FOUND);

        if (!string.IsNullOrWhiteSpace(categoria.NomeFoto))
        {
            var resultDeleteBlob = await _uploadImageBlobClient.DeleteImageAsync(categoria.NomeFoto);

            if (!resultDeleteBlob)
                throw new ExceptionApi("Não foi possível excluir a categoria!");
        }

        await _categoriaRepository.DeleteAsync(categoria);
    }

    public async Task<CategoriaViewModel> GetCategoriaAsync(Guid id)
    {
        var categoria = await _categoriaRepository.GetCategoriaAsync(id)
            ?? throw new ExceptionApi(ERRO_NOT_FOUND);

        return new CategoriaViewModel().ToModel(categoria);
    }

    public async Task<IList<CategoriaViewModel>> GetCategoriasAsync()
    {
        var categorias = await _categoriaRepository.GetCategoriasAsync();

        return categorias.Select(x => new CategoriaViewModel().ToModel(x)).ToList();
    }

    public async Task<PaginacaoViewModel<CategoriaViewModel>> GetPaginacaoAsync(FilterModel<Categoria> paginacaoCategoriaDto)
    {
        var paginacao = await _categoriaRepository.PaginacaoAsync(paginacaoCategoriaDto);

        return new PaginacaoViewModel<CategoriaViewModel>
        {
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => new CategoriaViewModel().ToModel(x)).ToList(),
            TotalDeRegistros = paginacao.TotalDeRegistros
        };
    }

    public async Task<CategoriaViewModel> UpdateCategoriaAsync(UpdateCategoriaDto updateCategoriaDto)
    {
        var categoria = await _categoriaRepository.GetCategoriaAsync(updateCategoriaDto.Id)
            ?? throw new ExceptionApi(ERRO_NOT_FOUND);

        var nomeFoto = categoria.NomeFoto;
        var foto = categoria.Foto;

        if (!string.IsNullOrWhiteSpace(updateCategoriaDto.NovaFoto))
        {
            if (!string.IsNullOrWhiteSpace(categoria.NomeFoto))
            {
                var resultDeleteBlob = await _uploadImageBlobClient.DeleteImageAsync(categoria.NomeFoto);
                if (!resultDeleteBlob)
                    throw new ExceptionApi("Não foi possível excluir a foto da categoria!");
            }

            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            foto = await _uploadImageBlobClient.UploadImageAsync(updateCategoriaDto.NovaFoto, nomeFoto);
        }

        categoria.Update(updateCategoriaDto.Descricao, foto, nomeFoto, updateCategoriaDto.InativoEcommerce);

        await _categoriaRepository.UpdateAsync(categoria);

        return new CategoriaViewModel().ToModel(categoria);
    }

    public async Task InativarAtivarEcommerceAsync(Guid id)
    {
        var categoria = await _categoriaRepository.GetCategoriaAsync(id)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro da categoria");

        categoria.InativarAtivarEcommerce();
        await _categoriaRepository.UpdateAsync(categoria);
    }

    public async Task<IList<CategoriaViewModel>> GetCategoriasDropDownAsync()
    {
        var categorias = await _categoriaRepository.GetCategoriasDropDownAsync();

        return categorias.Select(x => new CategoriaViewModel().ToModel(x)).ToList();
    }
}
