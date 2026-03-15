using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracaoDeFreteModel;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class ConfiguracaoDeFreteService : IConfiguracaoDeFreteService
{
    private readonly IConfiguracaoDeFreteRepository _configuracaoDeFreteRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;

    public ConfiguracaoDeFreteService(IConfiguracaoDeFreteRepository configuracaoDeFreteRepository,
        IParceiroAutenticado parceiroAutenticado)
    {
        _configuracaoDeFreteRepository = configuracaoDeFreteRepository;
        _parceiroAutenticado = parceiroAutenticado;
    }

    public async Task<ResultPartner<ConfiguracaoDeFreteViewModel>> ObterAsync()
    {
        var config = await _configuracaoDeFreteRepository.ObterDoParceiroAsync(_parceiroAutenticado.Id);
        if (config == null)
        {
            return (ResultPartner<ConfiguracaoDeFreteViewModel>)"Não há uma configuração cadastrada";
        }

        return (ResultPartner<ConfiguracaoDeFreteViewModel>)(ConfiguracaoDeFreteViewModel)config;
    }

    public async Task<ResultPartner<ConfiguracaoDeFreteViewModel>> CrairOuEditarAsync(
        ConfiguracaoDeFreteDTO configuracao)
    {
        var erro = configuracao.Validar();

        if (!string.IsNullOrWhiteSpace(erro))
        {
            return (ResultPartner<ConfiguracaoDeFreteViewModel>)erro;
        }

        var config = await _configuracaoDeFreteRepository.ObterDoParceiroAsync(_parceiroAutenticado.Id);
        if (config == null)
        {
            config = new ConfiguracaoDeFrete(id: Guid.NewGuid(), dataDeCriacao: DateTime.Now,
                dataDeAtualizacao: DateTime.Now, parceiroId: _parceiroAutenticado.Id, numero: 0)
            {
                Token = configuracao.Token,
                Ativo = configuracao.Ativo,
                CobrarCnpj = configuracao.CobrarCnpj,
                CobrarCpf = configuracao.CobrarCpf,
                CepOrigem = configuracao.CepOrigem,
            };

            await _configuracaoDeFreteRepository.AddAsync(config);
        }
        else
        {
            config.Token = configuracao.Token;
            config.Ativo = configuracao.Ativo;
            config.CobrarCnpj = configuracao.CobrarCnpj;
            config.CobrarCpf = configuracao.CobrarCpf;
            config.CepOrigem = configuracao.CepOrigem;

            _configuracaoDeFreteRepository.Update(config);
        }

        await _configuracaoDeFreteRepository.SaveChangesAsync();

        return (ResultPartner<ConfiguracaoDeFreteViewModel>)(ConfiguracaoDeFreteViewModel)config;
    }
}