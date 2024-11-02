using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Model;
using OpenAdm.Infra.Paginacao;

namespace OpenAdm.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioViewModel> GetUsuarioByIdAsync();
    Task<UsuarioViewModel> GetUsuarioByIdAdmAsync(Guid id);
    Task<IList<UsuarioViewModel>> GetAllUsuariosAsync();
    Task<ResponseLoginUsuarioViewModel> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto);
    Task<ResponseLoginUsuarioViewModel> UpdateUsuarioAsync(UpdateUsuarioDto updateUsuarioDto);
    Task TrocarSenhaAsync(UpdateSenhaUsuarioDto updateSenhaUsuarioDto);
    Task<PaginacaoViewModel<UsuarioViewModel>> PaginacaoAsync(PaginacaoUsuarioDto paginacaoUsuarioDto);
    Task<IList<UsuarioViewModel>> PaginacaoDropDownAsync(PaginacaoUsuarioDropDown paginacaoUsuarioDropDown);
    Task<bool> TemTelefoneCadastradoAsync();
}
