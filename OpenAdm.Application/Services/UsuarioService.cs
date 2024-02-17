using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Errors;
using OpenAdm.Domain.Exceptions;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly ITokenService _tokenService;

    public UsuarioService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
    {
        _usuarioRepository = usuarioRepository;
        _tokenService = tokenService;
    }

    public async Task<UsuarioViewModel> GetUsuarioByIdAsync()
    {
        var idToken = _tokenService.GetTokenUsuarioViewModel().Id;
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(idToken)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        return new UsuarioViewModel().ToModel(usuario);
    }

    public async Task<ResponseLoginUsuarioViewModel> UpdateUsuarioAsync(UpdateUsuarioDto updateUsuarioDto, ConfiguracaoDeToken configuracaoDeToken)
    {
        var idToken = _tokenService.GetTokenUsuarioViewModel().Id;
        var usuario = await _usuarioRepository.GetUsuarioByIdAsync(idToken)
            ?? throw new ExceptionApi(CodigoErrors.RegistroNotFound);

        usuario.Update(updateUsuarioDto.Email, updateUsuarioDto.Nome, updateUsuarioDto.Telefone, updateUsuarioDto.Cnpj);

        await _usuarioRepository.UpdateAsync(usuario);
        var usuarioViewModel = new UsuarioViewModel().ToModel(usuario);
        var token = _tokenService.GenerateToken(usuarioViewModel, configuracaoDeToken);

        return new(usuarioViewModel, token);
    }
}
