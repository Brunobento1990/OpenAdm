using OpenAdm.Application.Dtos.ConfiguracoesDeFreteDTO;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracaoDeFreteModel;
using OpenAdm.Application.Models.ConfiguracoesDePagamentos;
using OpenAdm.Domain.Entities;
using OpenAdm.Domain.Interfaces;
using OpenAdm.Domain.Model;

namespace OpenAdm.Application.Services;

public class ConfiguracaoDeFreteService : IConfiguracaoDeFreteService
{
    private readonly IConfiguracaoDeFreteRepository _configuracaoDeFreteRepository;
    private readonly IParceiroAutenticado _parceiroAutenticado;
    private readonly IUsuarioAutenticado _usuarioAutenticado;

    public ConfiguracaoDeFreteService(IConfiguracaoDeFreteRepository configuracaoDeFreteRepository,
        IParceiroAutenticado parceiroAutenticado, IUsuarioAutenticado usuarioAutenticado)
    {
        _configuracaoDeFreteRepository = configuracaoDeFreteRepository;
        _parceiroAutenticado = parceiroAutenticado;
        _usuarioAutenticado = usuarioAutenticado;
    }

    public async Task<EfetuarCobrancaViewModel> CobrarAsync()
    {
        var configuracao = await _configuracaoDeFreteRepository
            .ObterDoParceiroAsync(_usuarioAutenticado.ParceiroId);

        if (configuracao == null || !configuracao.Ativo || string.IsNullOrWhiteSpace(configuracao.Token))
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