using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IProdutoEcommerceRepository
{
    Task<PaginacaoViewModel<Produto>> ListarAsync(
        string? search,
        int page,
        ICollection<Guid>? categoriasIds);
}