using OpenAdm.Application.Adapters;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class AtualizarSenhaUsuarioAdmService : IAtualizarSenhaUsuarioAdmService
{
    private readonly IUsuarioRepository _usuarioRepository;

    public AtualizarSenhaUsuarioAdmService(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;
    }

    public async Task<bool> AtualizarAsync(AtualizarSenhaUsuarioAdmDto atualizarSenhaUsuarioAdmDto)
    {
        atualizarSenhaUsuarioAdmDto.Validar();
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(atualizarSenhaUsuarioAdmDto.UsuarioId)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro do usuário");

        usuario.UpdateSenha(PasswordAdapter.GenerateHash(atualizarSenhaUsuarioAdmDto.Senha));

        await _usuarioRepository.UpdateAsync(usuario);

        return true;
    }
}
