using Domain.Pkg.Entities;
using OpenAdm.Application.Models.Produtos;

namespace OpenAdm.Application.Interfaces;

public interface IProdutosMaisVendidosService
{
    Task<IList<ProdutoViewModel>> GetProdutosMaisVendidosAsync();
    Task ProcessarAsync(Pedido pedido);
}
