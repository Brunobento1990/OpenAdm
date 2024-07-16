using Domain.Pkg.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetUsuarioByIdAsync(Guid id);
    Task<Usuario?> GetUsuarioByEmailAsync(string email);
    Task<IList<Usuario>> GetAllUsuariosAsync();
    Task<PaginacaoViewModel<Usuario>> GetPaginacaoAsync(FilterModel<Usuario> filterModel);
}
