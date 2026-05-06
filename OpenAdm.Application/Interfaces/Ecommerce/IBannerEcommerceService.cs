using OpenAdm.Application.Queries;

namespace OpenAdm.Application.Interfaces.Ecommerce;

public interface IBannerEcommerceService
{
    Task<ICollection<BannerEcommerceQuery>> ListarTodosAsync(Guid parceiroId);
}