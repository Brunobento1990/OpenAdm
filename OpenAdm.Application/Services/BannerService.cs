using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Services;

public class BannerService
    : IBannerService
{
    private readonly IBannerRepository _bannerRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    private const string ERRO_NOT_FOUND = "Não foi possível localizar o banner!";

    public BannerService(IBannerRepository bannerRepository, IUploadImageBlobClient uploadImageBlobClient, IUsuarioAutenticado usuarioAutenticado)
    {
        _bannerRepository = bannerRepository;
        _uploadImageBlobClient = uploadImageBlobClient;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<BannerViewModel> CreateBannerAsync(BannerCreateDto bannerCreateDto)
    {
        bannerCreateDto.Validar();
        var nomeFoto = $"{Guid.NewGuid()}.jpeg";
        var foto = await _uploadImageBlobClient.UploadImageAsync(bannerCreateDto.NovaFoto, nomeFoto);
        var proximoNumero = await _bannerRepository.ProximoNumeroAsync(_usuarioAutenticado.ParceiroId);

        var banner = bannerCreateDto.ToEntity(nomeFoto, foto, _usuarioAutenticado.ParceiroId, proximoNumero);

        await _bannerRepository.AddAsync(banner);
        await _bannerRepository.SaveChangesAsync();
        return new BannerViewModel().ToModel(banner);
    }

    public async Task DeleteBannerAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi(ERRO_NOT_FOUND);

        var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(banner.NomeFoto);

        if (!resultBlobDelete)
            throw new ExceptionApi("Não foi possível excluir a foto do banner, tente novamente mais tarde, ou entre em contato com o suporte!");

        _bannerRepository.Delete(banner);
        await _bannerRepository.SaveChangesAsync();
    }

    public async Task<BannerViewModel> EditBannerAsync(BannerEditDto bannerEditDto)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(bannerEditDto.Id)
            ?? throw new ExceptionApi(ERRO_NOT_FOUND);

        var foto = banner.Foto;
        var nomeFoto = banner.NomeFoto;

        if (!string.IsNullOrWhiteSpace(bannerEditDto.NovaFoto))
        {
            if (!string.IsNullOrWhiteSpace(banner.NomeFoto))
            {
                var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(banner.NomeFoto);
                if (!resultBlobDelete)
                    throw new ExceptionApi("Não foi possível excluir a foto do banner, tente novamente mais tarde, ou entre em contato com o suporte!");
            }

            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            foto = await _uploadImageBlobClient.UploadImageAsync(bannerEditDto.NovaFoto, nomeFoto);
        }

        banner.Update(foto, nomeFoto, bannerEditDto.Ativo);

        _bannerRepository.Update(banner);
        await _bannerRepository.SaveChangesAsync();

        return new BannerViewModel().ToModel(banner);
    }

    public async Task<BannerViewModel> GetBannerByIdAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi(ERRO_NOT_FOUND);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task<IList<BannerViewModel>> GetBannersAsync()
    {
        var banners = await _bannerRepository.GetBannersAsync(_usuarioAutenticado.ParceiroId);

        return banners
            .Select(banner => new BannerViewModel().ToModel(banner))
            .ToList();
    }

    public async Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(FilterModel<Banner> paginacaoBannerDto)
    {
        paginacaoBannerDto.ParceiroId = _usuarioAutenticado.ParceiroId;
        var paginacao = await _bannerRepository.PaginacaoAsync(paginacaoBannerDto);

        return new()
        {
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => new BannerViewModel().ToModel(x)).ToList(),
            TotalDeRegistros = paginacao.TotalDeRegistros
        };
    }
}
