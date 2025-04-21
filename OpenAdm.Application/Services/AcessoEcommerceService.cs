using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class AcessoEcommerceService : IAcessoEcommerceService
{
    private readonly IAcessoEcommerceRepository _acessoEcommerceRepository;

    public AcessoEcommerceService(IAcessoEcommerceRepository acessoEcommerceRepository)
    {
        _acessoEcommerceRepository = acessoEcommerceRepository;
    }

    public async Task AtualizarAcessoAsync()
    {
        var dataAtual = DateTime.Now;
        var dataConsultaInicial = new DateTime(dataAtual.Year, dataAtual.Month, 1);
        var dataConsultaFinal = dataConsultaInicial.AddMonths(1).AddTicks(-1);

        var acesso = await _acessoEcommerceRepository.ObterPorDataAsync(dataConsultaInicial, dataConsultaFinal);

        if (acesso == null)
        {
            acesso = AcessoEcommerce.NovoAcesso(1);
            await _acessoEcommerceRepository.AddAsync(acesso);
            return;
        }

        acesso.AdicionarAcesso();
        await _acessoEcommerceRepository.EditAsync(acesso);
    }

    public async Task<long> QuantidadeDeAcessoAsync()
    {
        var dataAtual = DateTime.Now;
        var dataConsultaInicial = new DateTime(dataAtual.Year, dataAtual.Month, 1);
        var dataConsultaFinal = dataConsultaInicial.AddMonths(1).AddTicks(-1);

        var acesso = await _acessoEcommerceRepository.ObterPorDataAsync(dataConsultaInicial, dataConsultaFinal);
        return acesso?.Quantidade ?? 0;
    }
}
