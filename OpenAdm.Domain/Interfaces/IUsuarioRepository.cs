using OpenAdm.Domain.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetUsuarioByIdAsync(Guid id);
    Task<Usuario?> GetUsuarioByEmailAsync(string email);
    Task<Usuario?> GetUsuarioByCpfAsync(string cpf);
    Task<Usuario?> GetUsuarioByCnpjAsync(string cnpj);
    Task<IList<Usuario>> GetAllUsuariosAsync();
    Task<int> GetCountAsync();
}
