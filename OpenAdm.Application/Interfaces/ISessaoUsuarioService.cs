using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Application.Interfaces;

public interface ISessaoUsuarioService
{
    Task<SessaoUsuario> CriarAsync(Guid usuarioId, bool ehFuncionario);
    Task DerrubarSessaoAsync(Guid sessaoId);
    Task DerrubarSessoesAsync(Guid usuarioId, bool ehFuncionario);
}
