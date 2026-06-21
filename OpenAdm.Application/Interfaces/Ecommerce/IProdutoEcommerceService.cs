using OpenAdm.Application.Dtos.Produtos;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces.Ecommerce;

public interface IProdutoEcommerceService
{
    Task<ResultadoProdutoEcommerceModel> ListarAsync(ProdutoEcommerceFiltroDto filtro);
}