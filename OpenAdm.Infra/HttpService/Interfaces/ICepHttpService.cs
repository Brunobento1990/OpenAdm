using OpenAdm.Infra.Model;

namespace OpenAdm.Infra.HttpService.Interfaces;

public interface ICepHttpService
{
    Task<CotacaoFreteResponse> CotarFreteAsync(CotacaoFreteRequest cotacaoFreteRequest);
    Task<ConsultaCepResponse> ConsultaCepAsync(string cepOrigem);
}
