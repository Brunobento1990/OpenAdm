using OpenAdm.Application.Models;
using OpenAdm.Domain.Entities.OpenAdm;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IParcelaCobrancaService
{
    Task<PaginacaoViewModel<ParcelaCobrancaViewModel>> PaginacaoAsync(FilterModel<ParcelaCobranca> filter);
}