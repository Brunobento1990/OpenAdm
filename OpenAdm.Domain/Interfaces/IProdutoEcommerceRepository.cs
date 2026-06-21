using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IProdutoEcommerceRepository
{
    Task<ResultadoProdutoEcommerceModel> ListarAsync(
        string? search,
        int page,
        ICollection<Guid>? categoriasIds);
}