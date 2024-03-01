using OpenAdm.Application.Dtos.ConfiguracoesDeEmails;
using OpenAdm.Application.Interfaces;
using OpenAdm.Application.Models.ConfiguracoesDeEmails;
using OpenAdm.Domain.Interfaces;

namespace OpenAdm.Application.Services;

public class ConfiguracoesDeEmailService : IConfiguracoesDeEmailService
{
    private readonly IConfiguracaoDeEmailRepository _configuracaoDeEmailRepository;

    public ConfiguracoesDeEmailService(IConfiguracaoDeEmailRepository configuracaoDeEmailRepository)
    {
        _configuracaoDeEmailRepository = configuracaoDeEmailRepository;
    }

    public async Task<ConfiguracaoDeEmailViewModel> CreateConfiguracoesDeEmailAsync(CreateConfiguracoesDeEmailDto createConfiguracoesDeEmailDto)
    {
        var configuracao = createConfiguracoesDeEmailDto.ToEntity();

        await _configuracaoDeEmailRepository.AddAsync(configuracao);

        return new ConfiguracaoDeEmailViewModel().ToModel(configuracao);
    }
}
