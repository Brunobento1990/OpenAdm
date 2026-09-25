using OpenAdm.Application.Dtos.Representantes;
using OpenAdm.Application.Models;
using OpenAdm.Application.Models.Representantes;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Model;
using OpenAdm.Domain.PaginateDto;

namespace OpenAdm.Application.Interfaces;

public interface IRepresentanteService
{
    Task<ResultPartner<RepresentanteViewModel>> CriarAsync(CriarRepresentanteDto dto);
    Task<ResultPartner<RepresentanteViewModel>> EditarAsync(EditarRepresentanteDto dto);
    Task<ResultPartner<RepresentanteViewModel>> VisualizarAsync(Guid id);
    Task<ResultPartner<ResultadoPadraoViewModel>> InativarAtivarAsync(Guid id, bool ativo);
    Task<ResultPartner<PaginacaoViewModel<RepresentanteViewModel>>> PaginarAsync(FilterModel<Representante> filtro);
}
