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
        var acesso = await _acessoEcommerceRepository.ObterPorDataAsync(DateTime.Now.Year, DateTime.Now.Month);

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
        var acesso = await _acessoEcommerceRepository.ObterPorDataAsync(DateTime.Now.Year, DateTime.Now.Month);
        return acesso?.Quantidade ?? 0;
    }
}
