using OpenAdm.Application.Dtos.ConfiguracoesDePagamentos;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;
using OpenAdm.Domain.Helpers;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ConfiguracaoDePagamentoService : IConfiguracaoDePagamentoService
{
    private readonly IConfiguracaoDePagamentoRepository _configuracaoDePagamentoRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;
    public ConfiguracaoDePagamentoService(
        IConfiguracaoDePagamentoRepository configuracaoDePagamentoRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _configuracaoDePagamentoRepository = configuracaoDePagamentoRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<EfetuarCobrancaViewModel> CobrarAsync()
    {
        var configuracao = await _configuracaoDePagamentoRepository
            .GetAsync();

        if (configuracao == null)
        {
            return new()
            {
                Cobrar = false
            };
        }
        var usuario = await _usuarioAutenticado.GetUsuarioAutenticadoAsync();

        if (usuario.IsAtacado)
        {
            return new()
            {
                Cobrar = configuracao.CobrarCnpj
            };
        }

        return new()
        {
            Cobrar = configuracao.CobrarCpf
        };
    }

    public async Task<ConfiguracaoDePagamentoViewModel> CreateOrUpdateAsync(ConfiguracaoDePagamentoCreateOrUpdate configuracaoDePagamentoCreateOrUpdate)
    {
        var configuracao = await _configuracaoDePagamentoRepository
            .GetAsync();

        if (configuracao == null)
        {
            configuracao = new Domain.Entities.ConfiguracaoDePagamento(
                id: Guid.NewGuid(),
                dataDeCriacao: DateTime.Now,
                dataDeAtualizacao: DateTime.Now,
                numero: 0,
                publicKey: Criptografia.Encrypt(configuracaoDePagamentoCreateOrUpdate.PublicKey),
                accessToken: Criptografia.Encrypt(configuracaoDePagamentoCreateOrUpdate.AccessToken),
                cobrarCpf: configuracaoDePagamentoCreateOrUpdate.CobrarCpf,
                cobrarCnpj: configuracaoDePagamentoCreateOrUpdate.CobrarCnpj,
                urlWebHook: configuracaoDePagamentoCreateOrUpdate.UrlWebHook);

            await _configuracaoDePagamentoRepository.AddAsync(configuracao);
            return (ConfiguracaoDePagamentoViewModel)configuracao;
        }

        configuracao.Update(
            publicKey: Criptografia.Encrypt(configuracaoDePagamentoCreateOrUpdate.PublicKey),
            accessToken: Criptografia.Encrypt(configuracaoDePagamentoCreateOrUpdate.AccessToken),
            cobrarCpf: configuracaoDePagamentoCreateOrUpdate.CobrarCpf,
            cobrarCnpj: configuracaoDePagamentoCreateOrUpdate.CobrarCnpj,
            urlWebHook: configuracaoDePagamentoCreateOrUpdate.UrlWebHook);

        await _configuracaoDePagamentoRepository.UpdateAsync(configuracao);

        return (ConfiguracaoDePagamentoViewModel)configuracao;
    }

    public async Task<ConfiguracaoDePagamentoViewModel?> GetAsync()
    {
        var configuracao = await _configuracaoDePagamentoRepository
            .GetAsync();

        if (configuracao == null)
        {
            return null;
        }

        return (ConfiguracaoDePagamentoViewModel)configuracao;
    }
}
