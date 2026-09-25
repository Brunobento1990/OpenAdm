using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Interfaces;

public interface ISessaoUsuarioRepository
{
    Task AdicionarAsync(SessaoUsuario sessao);
    Task<SessaoUsuario?> ObterAsync(
        Guid sessaoId,
        Guid usuarioId,
        Guid parceiroId,
        TipoUsuario tipoUsuario);
    Task SalvarAlteracoesAsync();
    Task DerrubarSessaoAsync(Guid sessaoId);
    Task DerrubarSessoesAsync(Guid usuarioId, Guid parceiroId, TipoUsuario tipoUsuario);
}
