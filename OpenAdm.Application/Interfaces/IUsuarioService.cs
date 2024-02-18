using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Tokens;
using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioViewModel> GetUsuarioByIdAsync();
    Task<ResponseLoginUsuarioViewModel> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto, ConfiguracaoDeToken configuracaoDeToken);
    Task<ResponseLoginUsuarioViewModel> UpdateUsuarioAsync(UpdateUsuarioDto updateUsuarioDto, ConfiguracaoDeToken configuracaoDeToken);
}
