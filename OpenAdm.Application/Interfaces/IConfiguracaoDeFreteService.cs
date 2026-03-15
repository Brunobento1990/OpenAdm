using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.Models.ConfiguracaoDeFreteModel;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Interfaces;

public interface IConfiguracaoDeFreteService
{
    Task<ResultPartner<ConfiguracaoDeFreteViewModel>> ObterAsync();
    Task<ResultPartner<ConfiguracaoDeFreteViewModel>> CrairOuEditarAsync(ConfiguracaoDeFreteDTO configuracao);
}