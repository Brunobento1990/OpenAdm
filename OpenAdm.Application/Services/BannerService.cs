using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Application.HttpClient.Interfaces;
using OpenAdm.Domain.Entities;

namespace OpenAdm.Application.Services;

public class BannerService(IBannerRepository bannerRepository, IUploadImageBlobClient uploadImageBlobClient)
    : IBannerService
{
    private readonly IBannerRepository _bannerRepository = bannerRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient = uploadImageBlobClient;
    private const string ERRO_NOT_FOUND = "Não foi possível localizar o banner!";

    public async Task<BannerViewModel> CreateBannerAsync(BannerCreateDto bannerCreateDto)
    {
        bannerCreateDto.Validar();
        var nomeFoto = $"{Guid.NewGuid()}.jpeg";
        var foto = await _uploadImageBlobClient.UploadImageAsync(bannerCreateDto.NovaFoto, nomeFoto);

        var banner = bannerCreateDto.ToEntity(nomeFoto, foto);

        await _bannerRepository.AddAsync(banner);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task DeleteBannerAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi(ERRO_NOT_FOUND);

        var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(banner.NomeFoto);

        if (!resultBlobDelete)
            throw new ExceptionApi("Não foi possível excluir a foto do banner, tente novamente mais tarde, ou entre em contato com o suporte!");

        var result = await _bannerRepository.DeleteAsync(banner);

        if (!result) throw new ExceptionApi("Não foi possivel excluir o banner");
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

        await _bannerRepository.UpdateAsync(banner);

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
        var banners = await _bannerRepository.GetBannersAsync();

        return banners
            .Select(banner => new BannerViewModel().ToModel(banner))
            .ToList();
    }

    public async Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(FilterModel<Banner> paginacaoBannerDto)
    {
        var paginacao = await _bannerRepository.PaginacaoAsync(paginacaoBannerDto);

        return new()
        {
            TotalPaginas = paginacao.TotalPaginas,
            Values = paginacao.Values.Select(x => new BannerViewModel().ToModel(x)).ToList(),
            TotalDeRegistros = paginacao.TotalDeRegistros
        };
    }
}
