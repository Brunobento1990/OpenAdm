using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.Models.ConfiguracaoDeFreteModel;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IFreteService
{
    Task<ResultPartner<CotacaoDeFreteViewModel>> CotarFreteAsync(CotacaoFreteDTO cotacaoFreteDto);
}