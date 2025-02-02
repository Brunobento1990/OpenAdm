using OpenAdm.Application.Dtos.Usuarios;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Application.Models.Usuarios;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioViewModel> GetUsuarioByIdAsync();
    Task<UsuarioViewModel> GetUsuarioByIdAdmAsync(Guid id);
    Task<UsuarioViewModel> GetUsuarioByIdValidacaoAsync(Guid id);
    Task<IList<UsuarioViewModel>> GetAllUsuariosAsync();
    Task<ResponseLoginUsuarioViewModel> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto);
    Task<ResponseLoginUsuarioViewModel> UpdateUsuarioAsync(UpdateUsuarioDto updateUsuarioDto);
    Task TrocarSenhaAsync(UpdateSenhaUsuarioDto updateSenhaUsuarioDto);
    Task<PaginacaoViewModel<UsuarioViewModel>> PaginacaoAsync(FilterModel<Usuario> paginacaoUsuarioDto);
    Task<IList<UsuarioViewModel>> PaginacaoDropDownAsync(PaginacaoDropDown<Usuario> paginacaoUsuarioDropDown);
    Task<bool> TemTelefoneCadastradoAsync();
}
