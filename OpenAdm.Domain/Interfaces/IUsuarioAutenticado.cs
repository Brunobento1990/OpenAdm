using Domain.Pkg.Entities;

namespace OpenAdm.Domain.Interfaces;

public interface IUsuarioAutenticado
{
    Guid Id { get; set; }
    bool IsFuncionario { get; set; }
    Task<Usuario> GetUsuarioAutenticadoAsync();
}
