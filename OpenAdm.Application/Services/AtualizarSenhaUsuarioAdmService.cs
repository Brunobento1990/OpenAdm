using OpenAdm.Application.Adapters;
using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class AtualizarSenhaUsuarioAdmService : IAtualizarSenhaUsuarioAdmService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ISessaoUsuarioService _sessaoUsuarioService;

    public AtualizarSenhaUsuarioAdmService(
        IUsuarioRepository usuarioRepository,
        ISessaoUsuarioService sessaoUsuarioService)
    {
        _usuarioRepository = usuarioRepository;
        _sessaoUsuarioService = sessaoUsuarioService;
    }

    public async Task<bool> AtualizarAsync(AtualizarSenhaUsuarioAdmDto atualizarSenhaUsuarioAdmDto)
    {
        atualizarSenhaUsuarioAdmDto.Validar();
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(atualizarSenhaUsuarioAdmDto.UsuarioId)
            ?? throw new ExceptionApi("Não foi possível localizar o cadastro do usuário");

        usuario.UpdateSenha(PasswordAdapter.GenerateHash(atualizarSenhaUsuarioAdmDto.Senha));

        await _usuarioRepository.UpdateAsync(usuario);
        await _sessaoUsuarioService.DerrubarSessoesAsync(usuario.Id, ehFuncionario: false);

        return true;
    }
}
