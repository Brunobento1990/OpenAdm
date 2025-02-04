using OpenAdm.Application.Dtos.Usuarios;

namespace OpenAdm.Application.Interfaces;

public interface IAtualizarSenhaUsuarioAdmService
{
    Task<bool> AtualizarAsync(AtualizarSenhaUsuarioAdmDto atualizarSenhaUsuarioAdmDto);
}
