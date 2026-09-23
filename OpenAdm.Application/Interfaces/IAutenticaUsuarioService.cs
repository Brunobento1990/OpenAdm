using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IAutenticaUsuarioService
{
    Task<ResultPartner<bool>> ValidarAsync();
}