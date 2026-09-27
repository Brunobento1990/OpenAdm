using OpenAdm.Application.Dtos.Representantes;
using OpenAdm.Application.Models.Logins;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ILoginRepresentanteService
{
    Task<ResultPartner<ResponseLoginRepresentanteViewModel>> LoginAsync(LoginRepresentanteDto dto);
}
