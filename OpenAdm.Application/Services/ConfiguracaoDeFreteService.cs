using OpenAdm.Application.Dtos.ConfiguracoesDeFrete;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDeFrete;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public sealed class ConfiguracaoDeFreteService : IConfiguracaoDeFreteService
{
    private readonly IConfiguracaoDeFreteRepository _configuracaoDeFreteRepository;

    public ConfiguracaoDeFreteService(IConfiguracaoDeFreteRepository configuracaoDeFreteRepository)
    {
        _configuracaoDeFreteRepository = configuracaoDeFreteRepository;
    }

    public async Task<ConfiguracaoDeFreteViewModel> CreateOrUpdateAsync(
        ConfiguracaoDeFreteCreateDto configuracaoDeFreteCreateDto)
    {
        var configuracao = await _configuracaoDeFreteRepository.GetConfiguracaoAsync();

        if (configuracao == null) 
        {
            configuracao = configuracaoDeFreteCreateDto.ToEntity();
            await _configuracaoDeFreteRepository.AddAsync(configuracao);
            return (ConfiguracaoDeFreteViewModel)configuracao;
        }

        configuracao.Update(
            cepOrigem: configuracaoDeFreteCreateDto.CepOrigem,
            alturaEmbalagem: configuracaoDeFreteCreateDto.AlturaEmbalagem,
            larguraEmbalagem: configuracaoDeFreteCreateDto.LarguraEmbalagem,
            comprimentoEmbalagem: configuracaoDeFreteCreateDto.ComprimentoEmbalagem,
            peso: configuracaoDeFreteCreateDto.Peso);

        await _configuracaoDeFreteRepository.UpdateAsync(configuracao);

        return (ConfiguracaoDeFreteViewModel)configuracao;
    }

    public async Task<ConfiguracaoDeFreteViewModel> GetAsync()
    {
        var configuracao = await _configuracaoDeFreteRepository.GetConfiguracaoAsync();
        if (configuracao == null) return new();

        return (ConfiguracaoDeFreteViewModel)configuracao;
    }
}
