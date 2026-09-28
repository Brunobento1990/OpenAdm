using OpenAdm.Application.Models.Home;

namespace OpenAdm.Application.Interfaces;

public interface IResumoMensalHomeService
{
    Task<ResumoMensalHomeViewModel> ObterAsync();
    Task<ResumoMensalHomeViewModel> ObterAsync(Guid representanteId);
}
