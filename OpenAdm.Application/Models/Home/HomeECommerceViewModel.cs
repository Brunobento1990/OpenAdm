using OpenAdm.Application.Models.Banners;
using OpenAdm.Application.Models.Categorias;
using OpenAdm.Application.Models.Produtos;

namespace OpenAdm.Application.Models.Home;

public class HomeECommerceViewModel
{
    public IList<BannerViewModel> Banners { get; set; } = new List<BannerViewModel>();
    public IList<CategoriaViewModel> Categorias { get; set; } = new List<CategoriaViewModel>();
    public IList<ProdutoViewModel> ProdutosMaisVendidos { get; set; } = new List<ProdutoViewModel>();
}
