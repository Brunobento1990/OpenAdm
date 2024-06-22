using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Models.Usuarios;

public sealed class UsuarioAutenticado : IUsuarioAutenticado
{
    public Guid Id { get; set; }
    public DateTime DataDeAtualizacao { get ; set ; }
    public bool IsFuncionario { get; set; }
}
