using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;
using OpenAdm.Application.Dtos.Banners;
using OpenAdm.Application.Models.Banners;
using Domain.Pkg.Errors;
using Domain.Pkg.Exceptions;
using OpenAdm.Infra.Paginacao;
using OpenAdm.Infra.Azure.Interfaces;

namespace OpenAdm.Application.Services;

public class BannerService(IBannerRepository bannerRepository, IUploadImageBlobClient uploadImageBlobClient)
    : IBannerService
{
    private readonly IBannerRepository _bannerRepository = bannerRepository;
    private readonly IUploadImageBlobClient _uploadImageBlobClient = uploadImageBlobClient;

    public async Task<BannerViewModel> CreateBannerAsync(BannerCreateDto bannerCreateDto)
    {
        var nomeFoto = $"{Guid.NewGuid()}.jpeg";
        var foto = await _uploadImageBlobClient.UploadImageAsync(bannerCreateDto.Foto, nomeFoto);

        var banner = bannerCreateDto.ToEntity(nomeFoto, foto);

        await _bannerRepository.AddAsync(banner);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task DeleteBannerAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(banner.NomeFoto);

        if (!resultBlobDelete)
            throw new ExceptionApi("Não foi possível excluir a foto do banner, tente novamente mais tarde, ou entre em contato com o suporte!");

        var result = await _bannerRepository.DeleteAsync(banner);

        if (!result) throw new ExceptionApi();
    }

    public async Task<BannerViewModel> EditBannerAsync(BannerEditDto bannerEditDto)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(bannerEditDto.Id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        var foto = banner.Foto;
        var nomeFoto = banner.NomeFoto;

        if (!bannerEditDto.Foto.StartsWith("https://") && !string.IsNullOrWhiteSpace(bannerEditDto.Foto))
        {
            if (!string.IsNullOrWhiteSpace(banner.NomeFoto))
            {
                var resultBlobDelete = await _uploadImageBlobClient.DeleteImageAsync(banner.NomeFoto);
                if (!resultBlobDelete)
                    throw new ExceptionApi("Não foi possível excluir a foto do banner, tente novamente mais tarde, ou entre em contato com o suporte!");
            }

            nomeFoto = $"{Guid.NewGuid()}.jpeg";
            foto = await _uploadImageBlobClient.UploadImageAsync(bannerEditDto.Foto, nomeFoto);
        }

        banner.Update(foto, nomeFoto, bannerEditDto.Ativo);

        await _bannerRepository.UpdateAsync(banner);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task<BannerViewModel> GetBannerByIdAsync(Guid id)
    {
        var banner = await _bannerRepository.GetBannerByIdAsync(id)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        return new BannerViewModel().ToModel(banner);
    }

    public async Task<IList<BannerViewModel>> GetBannersAsync()
    {
        var banners = await _bannerRepository.GetBannersAsync();

        return banners
            .Select(banner => new BannerViewModel().ToModel(banner))
            .ToList();
    }

    public async Task<PaginacaoViewModel<BannerViewModel>> GetPaginacaoAsync(PaginacaoBannerDto paginacaoBannerDto)
    {
        var paginacao = await _bannerRepository.GetPaginacaoBannerAsync(paginacaoBannerDto);

        return new()
        {
            TotalPage = paginacao.TotalPage,
            Values = paginacao.Values.Select(x => new BannerViewModel().ToModel(x)).ToList()
        };
    }
}
