using OpenAdm.Domain.Enuns;

namespace OpenAdm.Application.Models.Tokens;

public class ValidaTokenModel
{
    public bool Expirado { get; set; }
    public Guid Id { get; set; }
    public Guid ParceiroId { get; set; }
    public Guid SessaoId { get; set; }
    public TipoUsuario TipoUsuario { get; set; }
    public DateTime DataDoLogin { get; set; }
}
