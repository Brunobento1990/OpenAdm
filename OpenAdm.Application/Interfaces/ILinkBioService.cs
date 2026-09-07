using OpenAdm.Application.Dtos.LinksBio;
using OpenAdm.Application.Models.LinksBio;
using OpenAdm.Application.Models;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface ILinkBioService
{
    Task<ResultPartner<LinkBioConfiguracaoViewModel>> ObterConfiguracaoAsync();
    Task<ResultPartner<LinkBioConfiguracaoViewModel>> CriarOuEditarConfiguracaoAsync(LinkBioConfiguracaoDto dto);
    Task<ResultPartner<LinkBioItemViewModel>> CriarLinkAsync(LinkBioItemCreateDto dto);
    Task<ResultPartner<LinkBioItemViewModel>> EditarLinkAsync(LinkBioItemUpdateDto dto);
    Task<ResultPartner<ResultadoPadraoViewModel>> ExcluirLinkAsync(Guid id);
    Task<ResultPartner<ResultadoPadraoViewModel>> AlterarStatusAsync(Guid id, bool ativo);
    Task<ResultPartner<ResultadoPadraoViewModel>> AlterarOrdemAsync(LinkBioAlterarOrdemDto dto);
    Task<ResultPartner<LinkBioPaginaPublicaViewModel>> ObterPaginaPublicaAsync();
    Task<ResultPartner<ResultadoPadraoViewModel>> RegistrarCliqueAsync(Guid linkId);
    Task<ResultPartner<LinkBioIndicadoresViewModel>> ConsultarEventosAsync(LinkBioEventosFiltroDto filtro);
}
