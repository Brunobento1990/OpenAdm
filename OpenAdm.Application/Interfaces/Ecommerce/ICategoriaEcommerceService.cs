using OpenAdm.Application.Queries;

namespace OpenAdm.Application.Interfaces.Ecommerce;

public interface ICategoriaEcommerceService
{
    Task<ICollection<CategoriaEcommerceQuery>> ListarTodasAsync();
    Task<ICollection<CategoriaEcommerceHomeQuery>> ListarHomeAsync();
}