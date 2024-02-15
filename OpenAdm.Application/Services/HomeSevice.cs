using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Application.Models.Home;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class HomeSevice : IHomeSevice
{
    private readonly ICategoriaRepository _categoryRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IBannerRepository _bannerRepository;

    public HomeSevice(
        ICategoriaRepository categoryRepository, 
        IProdutoRepository produtoRepository, 
        IBannerRepository bannerRepository)
    {
        _categoryRepository = categoryRepository;
        _produtoRepository = produtoRepository;
        _bannerRepository = bannerRepository;
    }

    public async Task<HomeECommerceViewModel> GetHomeEcommerceAsync()
    {
        var banners = await _bannerRepository.GetBannersAsync();
        var categorias = await _categoryRepository.GetCategoriasAsync();
        var produtosMaisVendidos = await _produtoRepository.GetProdutosMaisVendidosAsync();

        return new HomeECommerceViewModel()
        {
            Banners = banners.Select(x => new BannerViewModel().ToModel(x)).ToList(),
            Categorias = categorias.Select(x => new CategoriaViewModel().ToModel(x)).ToList(),
            ProdutosMaisVendidos = produtosMaisVendidos.Select(x => new ProdutoViewModel().ToModel(x)).ToList(),
        };
    }
}
