using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Interfaces;

public interface ISessaoUsuarioService
{
    Task<SessaoUsuario> CriarAsync(Guid usuarioId, TipoUsuario tipoUsuario);
    Task DerrubarSessaoAsync(Guid sessaoId);
    Task DerrubarSessoesAsync(Guid usuarioId, TipoUsuario tipoUsuario);
}
