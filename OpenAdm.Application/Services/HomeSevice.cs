using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Banners;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Application.Models.Home;
using OpenAdm.Application.Models.Produtos;
using OpenAdm.Application.Models.TopUsuario;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class HomeSevice : IHomeSevice
{
    private readonly ICategoriaRepository _categoryRepository;
    private readonly IProdutoRepository _produtoRepository;
    private readonly IBannerRepository _bannerRepository;
    private readonly ITopUsuariosRepository _topUsuariosRepository;

    public HomeSevice(
        ICategoriaRepository categoryRepository,
        IProdutoRepository produtoRepository,
        IBannerRepository bannerRepository,
        ITopUsuariosRepository topUsuariosRepository)
    {
        _categoryRepository = categoryRepository;
        _produtoRepository = produtoRepository;
        _bannerRepository = bannerRepository;
        _topUsuariosRepository = topUsuariosRepository;
    }

    public async Task<HomeAdmViewModel> GetHomeAdmAsync()
    {
        var topUsuariosTotalCompra = await _topUsuariosRepository.GetTopTresUsuariosToTalCompraAsync();
        var topUsuariosTotalPedido = await _topUsuariosRepository.GetTopTresUsuariosToTalPedidosAsync();

        return new HomeAdmViewModel()
        {
            TopUsuariosTotalCompra = topUsuariosTotalCompra.Select(x => (TopUsuariosViewModel)x).ToList(),
            TopUsuariosTotalPedido = topUsuariosTotalPedido.Select(x => (TopUsuariosViewModel)x).ToList()
        };
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
