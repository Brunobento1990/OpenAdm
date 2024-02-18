using OpenAdm.Application.Dtos.Usuarios;

namespace OpenAdm.Application.Interfaces;

public interface IEsqueceuSenhaService
{
    Task<bool> RecuperarSenhaAsync(EsqueceuSenhaDto esqueceuSenhaDto);
}
