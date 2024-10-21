using OpenAdm.Application.Dtos.ConfiguracoesDeFretes;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDeFretes;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ConfiguracaoDeFreteService : IConfiguracaoDeFreteService
{
    private readonly IConfiguracaoDeFreteRepository _configuracaoDeFreteRepository;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public ConfiguracaoDeFreteService(
        IConfiguracaoDeFreteRepository configuracaoDeFreteRepository,
        IUsuarioAutenticado usuarioAutenticado)
    {
        _configuracaoDeFreteRepository = configuracaoDeFreteRepository;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<EfetuarCobrancaViewModel> CobrarFreteAsync()
    {
        var configuracao = await _configuracaoDeFreteRepository.GetAsync();
        if (configuracao == null || (configuracao.Inativo.HasValue && configuracao.Inativo.Value) || string.IsNullOrWhiteSpace(configuracao.ChaveApi))
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
                Cobrar = configuracao.CobrarCnpj ?? false
            };
        }

        return new()
        {
            Cobrar = configuracao.CobrarCpf ?? false
        };
    }

    public async Task<ConfiguracaoDeFreteViewModel> CreateOrUpdateAsync(ConfiguracaoDeFreteCreateOrUpdateDto configuracaoDeFreteCreateOrUpdateDto)
    {
        var configuracao = await _configuracaoDeFreteRepository.GetAsync();

        if (configuracao == null)
        {
            configuracao = new Domain.Entities.ConfiguracaoDeFrete(
                id: Guid.NewGuid(),
                cepOrigem: configuracaoDeFreteCreateOrUpdateDto.CepOrigem,
                alturaEmbalagem: configuracaoDeFreteCreateOrUpdateDto.AlturaEmbalagem,
                larguraEmbalagem: configuracaoDeFreteCreateOrUpdateDto.LarguraEmbalagem,
                comprimentoEmbalagem: configuracaoDeFreteCreateOrUpdateDto.ComprimentoEmbalagem,
                chaveApi: configuracaoDeFreteCreateOrUpdateDto.ChaveApi,
                peso: configuracaoDeFreteCreateOrUpdateDto.Peso,
                cobrarCpf: configuracaoDeFreteCreateOrUpdateDto.CobrarCpf,
                cobrarCnpj: configuracaoDeFreteCreateOrUpdateDto.CobrarCnpj,
                inativo: false);

            await _configuracaoDeFreteRepository.AddAsync(configuracao);
            return (ConfiguracaoDeFreteViewModel)configuracao;
        }

        configuracao.Update(
            cepOrigem: configuracaoDeFreteCreateOrUpdateDto.CepOrigem,
            alturaEmbalagem: configuracaoDeFreteCreateOrUpdateDto.AlturaEmbalagem,
            larguraEmbalagem: configuracaoDeFreteCreateOrUpdateDto.LarguraEmbalagem,
            comprimentoEmbalagem: configuracaoDeFreteCreateOrUpdateDto.ComprimentoEmbalagem,
            chaveApi: configuracaoDeFreteCreateOrUpdateDto.ChaveApi,
            peso: configuracaoDeFreteCreateOrUpdateDto.Peso,
            cobrarCpf: configuracaoDeFreteCreateOrUpdateDto.CobrarCpf,
            cobrarCnpj: configuracaoDeFreteCreateOrUpdateDto.CobrarCnpj,
            inativo: configuracaoDeFreteCreateOrUpdateDto.Inativo);

        await _configuracaoDeFreteRepository.UpdateAsync(configuracao);

        return (ConfiguracaoDeFreteViewModel)configuracao;
    }

    public async Task<ConfiguracaoDeFreteViewModel> GetAsync()
    {
        var configuracao = await _configuracaoDeFreteRepository.GetAsync();

        if (configuracao == null)
        {
            return new();
        }

        return (ConfiguracaoDeFreteViewModel)configuracao;
    }
}
