using OpenAdm.Domain.Entities.OpenAdm;

namespace OpenAdm.Domain.Interfaces;

public interface ISessaoUsuarioRepository
{
    Task AdicionarAsync(SessaoUsuario sessao);
    Task<SessaoUsuario?> ObterAsync(
        Guid sessaoId,
        Guid usuarioId,
        Guid parceiroId,
        bool ehFuncionario);
    Task SalvarAlteracoesAsync();
    Task DerrubarSessaoAsync(Guid sessaoId);
    Task DerrubarSessaoUsuarioIdAsync(Guid usuarioId);
}
