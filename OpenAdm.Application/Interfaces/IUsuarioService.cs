using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;

namespace OpenAdm.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioViewModel> GetUsuarioByIdAsync();
    Task<IList<UsuarioViewModel>> GetAllUsuariosAsync();
    Task<ResponseLoginUsuarioViewModel> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto);
    Task<ResponseLoginUsuarioViewModel> UpdateUsuarioAsync(UpdateUsuarioDto updateUsuarioDto);
    Task TrocarSenhaAsync(UpdateSenhaUsuarioDto updateSenhaUsuarioDto);
}
