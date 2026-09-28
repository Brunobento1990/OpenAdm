using OpenAdm.Application.Models.Home;

namespace OpenAdm.Application.Interfaces;

public interface IHomeSevice
{
    Task<HomeAdmViewModel> GetHomeAdmAsync();
    Task<HomeRepresentanteViewModel> GetHomeRepresentanteAsync(Guid representanteId);
}
