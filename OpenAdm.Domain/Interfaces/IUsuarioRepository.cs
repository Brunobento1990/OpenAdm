using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;

namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioRepository : IGenericRepository<Usuario>
{
    Task<Usuario?> GetUsuarioByIdAsync(Guid id);
    Task<Usuario?> GetUsuarioByTokenEsqueceuSenhaAsync(Guid tokenEsqueceuSenha);
    Task<Usuario?> GetUsuarioByEmailAsync(string email);
    Task<Usuario?> GetUsuarioByCpfAsync(string cpf);
    Task<Usuario?> GetUsuarioByCnpjAsync(string cnpj);
    Task<IList<Usuario>> GetAllUsuariosAsync();
    Task<int> GetCountCpfAsync();
    Task<int> GetCountCnpjAsync();
    Task<IList<Usuario>> UsuariosSemPedidoAsync();
    Task AddEnderecoAsync(EnderecoUsuario endereco);
    void EditarEndereco(EnderecoUsuario endereco);
    Task<EnderecoUsuario?> ObterEnderecoAsync(Guid usuarioId);
    Task<PaginacaoViewModel<Usuario>> ListarUltimoPedidoAsync(int page, bool isJuridico, string? search);
}
