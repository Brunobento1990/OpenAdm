using OpenAdm.Application.Interfaces;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class AcessoEcommerceService : IAcessoEcommerceService
{
    private readonly IAcessoEcommerceRepository _acessoEcommerceRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public AcessoEcommerceService(IAcessoEcommerceRepository acessoEcommerceRepository, IUsuarioAutenticado usuarioAutenticado)
    {
        _acessoEcommerceRepository = acessoEcommerceRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task AtualizarAcessoAsync()
    {
        var acesso = await _acessoEcommerceRepository.ObterPorDataAsync(DateTime.Now.Year, DateTime.Now.Month, _usuarioAutenticado.ParceiroId);

        if (acesso == null)
        {
            acesso = AcessoEcommerce.NovoAcesso(1, _usuarioAutenticado.ParceiroId);
            await _acessoEcommerceRepository.AddAsync(acesso);
            return;
        }

        acesso.AdicionarAcesso();
        await _acessoEcommerceRepository.EditAsync(acesso);
    }

    public async Task<long> QuantidadeDeAcessoAsync()
    {
        var acesso = await _acessoEcommerceRepository.ObterPorDataAsync(DateTime.Now.Year, DateTime.Now.Month, _usuarioAutenticado.ParceiroId);
        return acesso?.Quantidade ?? 0;
    }
}
