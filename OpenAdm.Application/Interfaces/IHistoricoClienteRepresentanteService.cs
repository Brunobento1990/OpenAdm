using OpenAdm.Application.Models.Representantes;

namespace OpenAdm.Application.Interfaces;

public interface IHistoricoClienteRepresentanteService
{
    Task<HistoricoClienteRepresentanteViewModel> ObterAsync(Guid clienteId, Guid representanteId);
}
