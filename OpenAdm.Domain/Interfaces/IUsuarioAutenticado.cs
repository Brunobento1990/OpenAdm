using OpenAdm.Domain.Entities;

using OpenAdm.Domain.Enuns;

namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioAutenticado
{
    Guid Id { get; set; }
    Guid SessaoId { get; set; }
    Guid ParceiroId { get; set; }
    TipoUsuario TipoUsuario { get; set; }
    Task<Usuario> GetUsuarioAutenticadoAsync();
    Task<Usuario?> GetUsuarioMiddlewareAsync();
    Task<Funcionario?> GetFuncionarioMiddlewareAsync();
    Task<Representante?> GetRepresentanteMiddlewareAsync();
    Task<Usuario?> GetUsuarioAutenticadoOrNullAsync();
}
